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
    }
    public override void Enter()
    {
        gameManager.playerController.transform.position = defaultSpawnPosition.position;
    }
    public override void Exit()
    {

    }
    #endregion

    public GameObject pauseUI;
    public Transform defaultSpawnPosition;

    private bool paused;

    void Awake()
    {
        PlayerController.PlayerPressedStartEvent += PlayerPressedStartHandler;
    }

    void Start()
    {
        OnGameplayLoaded.Invoke(this);
    }

    private void PlayerDiedHandler(PlayerController sourcePlayer)
    {

    }

    private void PlayerPressedStartHandler(PlayerController p)
    {
        //UISfx.Instance.PlayBack();
        if (paused)
        {
            pauseUI.SetActive(false);
            Unpause();
        }
        else
        {
            pauseUI.SetActive(true);
            Pause();
        }
    }

    public void Pause()
    {
        //gameCamera.movementEnabled = false;//Stops camera shake when paused
        // foreach(PlayerController p in playerControllers.Values)
        // {
        //     p.Pause();
        // }
        Time.timeScale = 0;
        paused = true;
    }


    public void Unpause()
    {
        // gameCamera.movementEnabled = true;
        // foreach(PlayerController p in playerControllers.Values)
        // {
        //     p.Unpause();
        // }
        Time.timeScale = 1;
        paused = false;
    }
}