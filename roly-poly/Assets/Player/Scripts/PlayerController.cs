using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Static Events
    public static event Action<PlayerController> PlayerPressedStartEvent = delegate { };
    public static event Action<PlayerController> PlayerWonEvent = delegate { };
    public static event Action<PlayerController> PlayerDeadEvent = delegate { };
    public static event Action<PlayerController> PlayerUpdateHealth = delegate { };
    public static event Action<AbilitiesToUnlock> PlayerUnlockedAbility = delegate { };
    public static event Action<PlayerController> PlayerInstantiatedEvent = delegate { };
    #endregion

    #region Constants
    public int HEALTH_DEFAULT;
    public float INVINCIBLE_DURATION;
    #endregion
    public PlayerPhysics physics;
    public PlayerAnimations animations;
    public PlayerAbilities abilities;
    public GameObject model;
    [HideInInspector]
    public int currentHealth;
    [HideInInspector]
    public int maxHealth;

    public bool isDebug;
    public PlayerInput playerInput;

    public struct Inputs
    {
        public float horz;
        public float vert;
        public bool dribble;
        public bool boostBall;
        public bool releaseBoostBall;
        public bool bugBlast;
        public bool switchMode;

    }
    public Inputs inputs;
    public bool isInvincible;


    private PlayerState state;

    [SerializeField]
    private StateID stateID;

    private bool pause;

    public struct AnimTransition
    {
        public string name;
        public int priority;
        //Higher value means higher priority
        //Priority for most states should be 0
        //Priority of an attack should be 1
        //Priority of state that is forced from an outside source should be 2
        public AnimTransition(string name, int priority)
        {
            this.name = name;
            this.priority = priority;
        }

    }
    private AnimTransition nextAnim = new AnimTransition(null, 0);

    void Awake()
    {
        abilities.SetPlayerController(this);
        physics.SetPlayerController(this);
        state = new IdleState(this);
        stateID = state.GetStateID();
        pause = false;
        currentHealth = HEALTH_DEFAULT;
        maxHealth = HEALTH_DEFAULT;
        if (isDebug)
        {
            playerInput.enabled = true;
            abilities.UnlockAll();
        }
        else
        {
            Destroy(playerInput);
        }
    }
    void Start()
    {
        PlayerUpdateHealth(this);
        if (GlobalSFX.Instance)
        {
            GlobalSFX.Instance.PlayGameStart();
        }
        state.StateEnter();
    }

    public void CheckNewState(PlayerState newState)
    {
        if (newState == null || newState.GetStateID() == state.GetStateID())
            return;
        ChangeState(newState);
    }

    private void ChangeState(PlayerState newState)
    {
        // Debug.Log(string.Format("state {0} to {1}", state.GetStateID(), newState.GetStateID()));
        state.StateExit();
        state = newState;
        stateID = state.GetStateID();//For debugging purposes
        state.StateEnter();
    }

    //Use fixed update because the ecb moves in fixed update
    void FixedUpdate()
    {
        model.transform.position = physics.rb.transform.position;
        if (!physics.IsRoll())
            model.transform.localRotation = physics.rb.transform.localRotation;
        if (!pause)
        {
            animations.ClearAnimTriggers();
            CheckNewState(state.Update());
            StartNextAnim();
        }
    }

    void HandleInput()
    {
        if (!pause)
        {
            // State can be null if player joins scene by pressing input.
            // The input events will be fired before Start() is called.
            if (state == null)
            {
                return;
            }
            CheckNewState(state.HandleInput());
            ClearInputs();
        }
    }
    private void ClearInputs()
    {
        inputs.dribble = false;
        inputs.switchMode = false;
        inputs.boostBall = false;
        inputs.releaseBoostBall = false;
        inputs.bugBlast = false;
    }



    public void SetNextAnim(string name, int priority = 0)
    {
        if (this.nextAnim.name == null || priority > this.nextAnim.priority)
        {
            this.nextAnim = new AnimTransition(name, priority);
        }
    }

    private void StartNextAnim()
    {
        if (nextAnim.name != null)
        {
            // Debug.Log("Start " + this.nextAnim.name);
            animations.Play(nextAnim.name);
        }
        nextAnim.name = null;
        nextAnim.priority = 0;
    }



    #region Input Handling
    public void OnStick(Vector2 dir)
    {
        inputs.horz = dir.x;
        inputs.vert = dir.y;
        HandleInput();
    }

    public void OnSwitchMode()
    {
        inputs.switchMode = true;

        HandleInput();
    }

    public void OnDribble()
    {
        inputs.dribble = true;
        HandleInput();
    }

    public void OnStart()
    {
        PlayerPressedStartEvent(this);
    }
    public void OnBoostBall()
    {
        inputs.boostBall = true;
        HandleInput();
    }

    public void OnReleaseBoostBall(float duration)
    {
        inputs.releaseBoostBall = true;
        HandleInput();
    }

    public void OnBugBlast()
    {
        inputs.bugBlast = true;
        HandleInput();
    }

    #endregion

    #region Helpers
    public void CheckFlip(float horz)
    {
        if ((physics.GetFacingDir() == 1 && horz < 0) || (physics.GetFacingDir() == -1 && horz > 0))
        {
            animations.Flip(physics.GetFacingDir());
            physics.FlipFacingDirection();
        }
    }
    public bool IsInputHorz()
    {
        if (Mathf.Abs(inputs.horz) > 0)
        {
            return true;
        }
        else
            return false;
    }

    #endregion

    public void SetPause(bool pause)
    {
        this.pause = pause;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (GlobalSFX.Instance)
        {
            GlobalSFX.Instance.PlayPlayerHurt();
        }
        PlayerUpdateHealth(this);
        if (currentHealth <= 0)
        {
            if (GlobalSFX.Instance)
            {
                GlobalSFX.Instance.PlayPlayerDead();
            }
            PlayerDeadEvent.Invoke(this);

        }
    }
    public void SetInvicible(bool invincible)
    {
        this.isInvincible = invincible;
    }
    public void SetTempInvincible()
    {
        SetInvicible(true);
        StartCoroutine(StartInvincibleReset());
    }

    private IEnumerator StartInvincibleReset()
    {
        float timer = 0;
        Color playerColor = animations.sprite.color;
        Color flashColor = playerColor;
        flashColor.a = 0.25f;
        while (timer < INVINCIBLE_DURATION)
        {
            animations.sprite.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            animations.sprite.color = playerColor;
            timer += 0.1f;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }
        SetInvicible(false);
    }
    public bool IsInvincible()
    {
        return isInvincible;
    }

    public void UnlockAbility(AbilitiesToUnlock ability)
    {
        abilities.Unlock(ability);
        PlayerUnlockedAbility.Invoke(ability);
    }

    public void LoadPlayerAbilities(SaveData saveData)
    {
        if (saveData.dribbleUnlock)
        {
            abilities.Unlock(AbilitiesToUnlock.Dribble);
            PlayerUnlockedAbility.Invoke(AbilitiesToUnlock.Dribble);
        }
        if (saveData.boostBallUnlock)
        {
            abilities.Unlock(AbilitiesToUnlock.BoostBall);
            PlayerUnlockedAbility.Invoke(AbilitiesToUnlock.BoostBall);
        }
        if (saveData.stickyFeetUnlock)
        {
            abilities.Unlock(AbilitiesToUnlock.StickyFeet);
            PlayerUnlockedAbility.Invoke(AbilitiesToUnlock.StickyFeet);
        }
        if (saveData.bugBlastUnlock)
        {
            abilities.Unlock(AbilitiesToUnlock.BugBlast);
            PlayerUnlockedAbility.Invoke(AbilitiesToUnlock.BugBlast);
        }

    }
}
