using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : ActionableState
{
    public IdleState(PlayerController p) : base(p, StateID.Idle) { }

    public override PlayerState Update()
    {
        if(p.IsInputHorz())
        {
            return new WalkingState(p);
        }
        return base.Update();
    }
    public override void StateEnter()
    {
        p.SetNextAnim("Idle");
    }
}