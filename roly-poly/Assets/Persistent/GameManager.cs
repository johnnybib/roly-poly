using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    #region singleton
    private static GameManager instance; //To check if already exists. If it does don't create a new one
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    #region Gamestate
    public class GameState
    {
        public UnityEvent OnEnter = new UnityEvent();
        public UnityEvent OnExit = new UnityEvent();

        public string gameStateName;
        public GameState(string gameStateName)
        {
            this.gameStateName = gameStateName;
        }
    }

    public class GameStates
    {
        public GameState MainMenu = new GameState("MainMenu");
        public GameState Gameplay = new GameState("Gameplay");
        public GameState End = new GameState("End");
    }

    public GameStates gameStates;
    public string currentStateName;
    private GameState stateOnSceneLoad;
    private GameState currentState;

    #endregion GameState

    public GameObject loadingScreen;
    public PersistentPlayerController pp;
    private bool loadGameplayWithSaveData = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        MainMenuController.OnMainMenuLoaded += GameStateLoadedHandler;
        GameplayController.OnGameplayLoaded += GameplayStartedHandler;
        GameEndController.OnGameEndLoaded += GameStateLoadedHandler;
        SceneManager.sceneLoaded += CheckIfMenuLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        gameStates = new GameStates();
        stateOnSceneLoad = gameStates.MainMenu;

        loadingScreen.SetActive(false);

    }

    #region State Loading Handlers
    //Called for all start functions for each GameStateController in the scene (third)
    public void GameStateLoadedHandler(GameStateController gsController)
    {
        gsController.SetGameManager(this);
        gsController.SetupEventListeners();
        OnFinishedLoadingState();
    }

    public void GameplayStartedHandler(GameplayController gpc)
    {

    }
    public void OnFinishedLoadingState()
    {
        currentState.OnEnter.Invoke();
    }
    private void CheckIfMenuLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            SetCurrentState(stateOnSceneLoad);
        }
    }
    #endregion

    #region Changing State

    public void StartGameplay(bool loadWithSaveData = false)
    {
        loadGameplayWithSaveData = loadWithSaveData;
        LoadScene("Gameplay");
    }

    public void EndGameplay()
    {
        // End or main menu
        //LoadScene("GameEnd");
        //LoadScene("MainMenu");
    }

    public void QuitEndGame()
    {
        //LoadScene("MainMenu");
    }

    public void SetCurrentState(GameState newState)
    {
        currentState = newState;
        currentStateName = newState.gameStateName;
    }

    #endregion

    #region Scene Loading
    private void LoadScene(string sceneName)
    {
        pp.SetCanInput(false);
        loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync(sceneName);
    }
    private void OnSceneLoaded(Scene sceneLoaded, LoadSceneMode loadSceneMode)
    {
        Debug.Log("scene loaded");
        pp.SetCanInput(true);
        loadingScreen.SetActive(false);
    }
    #endregion
}
