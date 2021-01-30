using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StateID { Null, Idle, Rolling, Walking, Falling, Rising, Dribble} 
public abstract class PlayerState
{
    protected PlayerController p;
    protected StateID stateID;
    public PlayerState(PlayerController p, StateID stateID)
    {
        this.p = p;
        this.stateID = stateID;
    }
    public abstract PlayerState HandleInput();
    public abstract PlayerState Update();

    //To call from PlayerController at the end of the frame if the state changes
    public virtual void StateEnter(){ }

    public virtual void StateExit(){ }

    public StateID GetStateID()
    {
        return stateID;
    }

}