using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundedState : ActionableState
{
    public GroundedState(PlayerController p, StateID stateID) : base(p, stateID) { }

    public override PlayerState HandleInput()
    {
        // PlayerState returnVal = base.HandleInput();
        // if (returnVal != null) return returnVal;
        // return p.SwitchGroundedState();
        return null;
    }

    public override PlayerState Update()
    {
        // if (!p.physics.IsGrounded())
        // {
        //     return new FallingState(p);
        // }
        return null;
    }
}
