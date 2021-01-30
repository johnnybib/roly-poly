using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateController : MonoBehaviour
{
    protected GameManager gameManager;
    public virtual void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    //Called from game manager on start
    //Set the functions to be called from game manager events
    public abstract void SetupEventListeners();

    //Called when game state is entered
    public abstract void Enter();

    //Called when game state is exited
    public abstract void Exit();

}