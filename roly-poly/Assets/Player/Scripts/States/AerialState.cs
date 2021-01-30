using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialState : ActionableState
{
    public AerialState(PlayerController p, StateID stateID) : base(p, stateID) { }
    public override PlayerState HandleInput()
    {
        return base.HandleInput();
    }

    public override PlayerState Update()
    {
        return null;
        // if (p.physics.IsGrounded())
        // {
        //     PlayerState nextState = p.SwitchGroundedState();
        //     if(nextState != null)
        //         return nextState;
        // }
        // if (p.physics.IsFalling())
        // {
        //     return new FallingState(p);
        // }
        // else
        // {
        //     return new RisingState(p);
        // }
    }
}
