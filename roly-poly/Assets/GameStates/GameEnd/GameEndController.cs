using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndController : GameStateController
{
    #region GameState methods
    public static event System.Action<GameEndController> OnGameEndLoaded = delegate { };

    public override void SetupEventListeners()
    {
        //Setup event listeners for button presses
        gameManager.gameStates.MainMenu.OnEnter.AddListener(Enter);
        gameManager.gameStates.MainMenu.OnExit.AddListener(Exit);
    }
    public override void Enter()
    {
        this.gameObject.SetActive(true);
    }
    public override void Exit()
    {
        this.gameObject.SetActive(false);
    }
    void OnDestroy()
    {
        if (gameManager == null)
            return;
        gameManager.gameStates.MainMenu.OnEnter.RemoveListener(Enter);
        gameManager.gameStates.MainMenu.OnExit.RemoveListener(Exit);
    }
    #endregion GameState methods

    void Start()
    {
        OnGameEndLoaded(this);
    }

    public void ToMainMenu()
    {
        gameManager.ExitGameEnd();
    }
}
