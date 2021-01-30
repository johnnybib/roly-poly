using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : GameStateController
{
    #region GameState methods
    public static event System.Action<MainMenuController> OnMainMenuLoaded = delegate { };

    public override void SetupEventListeners()
    {
        //Setup event listeners for button presses
        gameManager.gameStates.MainMenu.OnEnter.AddListener(Enter);
        gameManager.gameStates.MainMenu.OnExit.AddListener(Exit);
    }
    public override void Enter()
    {
        this.gameObject.SetActive(true);
        // Set load button to false
        mainMenuUI.SetIsLoadGameActive(SaveSystem.DoesSaveFileExist());

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

    public MainMenuUIController mainMenuUI;

    void Start()
    {
        this.gameObject.SetActive(false);
        OnMainMenuLoaded(this);
    }

    public void HandleNewGame()
    {
        Debug.Log("New Game");
        gameManager.StartGameplay();
    }

    public void HandleLoadGame()
    {
        Debug.Log("Load Game");
        gameManager.StartGameplay(true);
    }

    public void HandleQuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
