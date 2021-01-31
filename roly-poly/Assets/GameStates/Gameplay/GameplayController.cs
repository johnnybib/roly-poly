using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : GameStateController
{
    #region GameState methods

    public static event System.Action<GameplayController> OnGameplayLoaded = delegate { };
    public override void SetupEventListeners()
    {
        //Setup event listeners for button presses
        gameManager.gameStates.Gameplay.OnEnter.AddListener(Enter);
        gameManager.gameStates.Gameplay.OnExit.AddListener(Exit);
    }
    void OnDestroy()
    {
        if (paused)
        {
            Unpause();
        }
        PlayerController.PlayerPressedStartEvent -= PlayerPressedStartHandler;
        PlayerController.PlayerDeadEvent -= PlayerDiedHandler;
        PlayerController.PlayerUpdateHealth -= PlayerUpdateHealthHandler;
        PlayerController.PlayerUnlockedAbility -= PlayerUnlockedAbilityHandler;
        PlayerController.PlayerGotEgg -= PlayerGotEggHandler;


    }
    public override void Enter()
    {
        ResetPlayer();
    }
    public override void Exit()
    {

    }
    #endregion

    public GameObject pauseUI;
    public GameObject deadUI;

    public HudController hudController;
    public Transform defaultSpawnPosition;

    public DeathLava deathLava;

    public AudioSource levelStartMusic;

    public AudioSource lavaMusic;

    private bool paused;

    void Awake()
    {
        PlayerController.PlayerPressedStartEvent += PlayerPressedStartHandler;
        PlayerController.PlayerDeadEvent += PlayerDiedHandler;
        PlayerController.PlayerUpdateHealth += PlayerUpdateHealthHandler;
        PlayerController.PlayerUnlockedAbility += PlayerUnlockedAbilityHandler;
        PlayerController.PlayerGotEgg += PlayerGotEggHandler;


        pauseUI.SetActive(false);
        deadUI.SetActive(false);
    }

    void Start()
    {
        OnGameplayLoaded.Invoke(this);
    }

    private void PlayerDiedHandler(PlayerController sourcePlayer)
    {
        gameManager.playerController.SetPause(true);
        deadUI.SetActive(true);
        Debug.Log("Player Died");
    }

    private void PlayerPressedStartHandler(PlayerController p)
    {
        //UISfx.Instance.PlayBack();
        if (paused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    private void PlayerUpdateHealthHandler(PlayerController p)
    {
        hudController.SetMaxHealth(p.maxHealth);
        hudController.SetCurrentHealth(p.currentHealth);
    }

    private void PlayerUnlockedAbilityHandler(AbilitiesToUnlock ability)
    {
        hudController.UnlockAbility(ability);
    }

    private void PlayerGotEggHandler(PlayerController p)
    {
        levelStartMusic.Stop();
        lavaMusic.Play();
        deathLava.StartRising();
    }

    public void Pause()
    {
        //gameCamera.movementEnabled = false;//Stops camera shake when paused
        pauseUI.SetActive(true);
        gameManager.playerController.SetPause(true);
        Time.timeScale = 0;
        paused = true;
    }


    public void Unpause()
    {
        // gameCamera.movementEnabled = true;
        pauseUI.SetActive(false);
        gameManager.playerController.SetPause(false);
        Time.timeScale = 1;
        paused = false;
    }

    public void SaveAndQuit()
    {
        Debug.Log("Save is disabled!");
        //Save();
        gameManager.EndGameplay();
    }

    public void ReloadGameplay()
    {
        gameManager.StartGameplay(true);
    }

    public void Save()
    {
        Debug.Log("Saving...");
        SaveSystem.SavePlayerControllerData(gameManager.playerController);
        Debug.Log("Saved!");
    }

    public void Win()
    {
        Debug.Log("You Win!");
        gameManager.EndGameplay(true);
    }

    public void ResetPlayer()
    {
        gameManager.playerController.transform.position = defaultSpawnPosition.position;
        gameManager.playerController.SetPause(false);

        if (gameManager.loadGameplayWithSaveData)
        {
            Debug.Log("Loading from saved data");
            SaveData saveData = SaveSystem.LoadFile();
            if (saveData != null)
            {
                gameManager.playerController.LoadPlayerAbilities(saveData);
            }
        }

    }

    public void DebugDoDamageToPlayer()
    {
        gameManager.playerController.TakeDamage(1);
    }
}