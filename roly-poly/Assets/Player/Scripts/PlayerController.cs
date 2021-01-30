using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerPhysics physics;
    public PlayerAnimations animations;
    public PlayerAbilities abilities;
    public GameObject model;

    public struct Inputs{
        public float horz;
        public float vert;
        public bool dribble;
        public bool boostBall;
        public bool releaseBoostBall;
        public bool switchMode;
    }
    public Inputs inputs;

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
        abilities.UnlockAll();
        state = new IdleState(this);
        stateID = state.GetStateID();
        pause = false;
    }    
    void Start()
    {
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
        Debug.Log(string.Format("state {0} to {1}", state.GetStateID(), newState.GetStateID()));
        state.StateExit();
        state = newState;
        stateID = state.GetStateID();//For debugging purposes
        state.StateEnter();
    }

    //Use fixed update because the ecb moves in fixed update
    void FixedUpdate()
    {
        model.transform.position = physics.rb.transform.position;   
        if(!physics.IsRoll()) 
            model.transform.localRotation = physics.rb.transform.localRotation;
        if(!pause)
        {
            animations.ClearAnimTriggers();
            CheckNewState(state.Update());
            ClearInputs();
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
        }
    }
    private void ClearInputs()
    {
        inputs.horz = 0;
        inputs.vert = 0;
        inputs.dribble = false;
        inputs.switchMode = false;
        inputs.boostBall = false;
        inputs.releaseBoostBall = false;
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
        if (nextAnim.name != null )
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

    #endregion

    #region Helpers
    public void CheckFlip(float horz, bool stop = true)
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
}
