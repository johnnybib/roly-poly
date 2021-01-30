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
    private GameState currentState;

    #endregion GameState

    public GameObject loadingScreen;
    public PersistentPlayerController pp;
    [HideInInspector]
    public PlayerController playerController;
    public GameObject playerPrefab;
    [HideInInspector]
    public bool loadGameplayWithSaveData = false;

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

        loadingScreen.SetActive(false);
        pp.SetCanInput(false);
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
        playerController = Instantiate(playerPrefab).GetComponent<PlayerController>();
        pp.SetPlayerController(playerController);
        pp.SetCanInput(true);

        gpc.SetGameManager(this);
        gpc.SetupEventListeners();
        currentState.OnEnter.Invoke();
    }
    public void OnFinishedLoadingState()
    {
        currentState.OnEnter.Invoke();
    }
    private void CheckIfMenuLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            SetCurrentState(gameStates.MainMenu);
        }
    }
    #endregion

    #region Changing State

    public void StartGameplay(bool loadWithSaveData = false)
    {
        currentState.OnExit.Invoke();

        loadGameplayWithSaveData = loadWithSaveData;
        SetCurrentState(gameStates.Gameplay);
        LoadScene("Gameplay");

    }

    public void EndGameplay(bool didWin = false)
    {
        currentState.OnExit.Invoke();

        pp.SetCanInput(false);
        if (didWin)
        {
            SetCurrentState(gameStates.End);
            LoadScene("GameEnd");
        }
        else
        {
            SetCurrentState(gameStates.MainMenu);
            LoadScene("MainMenu");
        }
    }

    public void ExitGameEnd()
    {
        currentState.OnExit.Invoke();
        SetCurrentState(gameStates.MainMenu);
        LoadScene("MainMenu");
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
        loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync(sceneName);
    }
    private void OnSceneLoaded(Scene sceneLoaded, LoadSceneMode loadSceneMode)
    {
        Debug.Log("scene loaded");
        loadingScreen.SetActive(false);
    }
    #endregion
}
