using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : ActionableState
{
    public WalkingState(PlayerController p) : base(p, StateID.Walking) { }
    public override PlayerState Update()
    {
        p.CheckFlip(p.inputs.horz);
        if (p.IsInputHorz())
        {
            p.physics.Walk(p.inputs.horz);
        }
        else 
        {
            return new IdleState(p);
        }
        return base.Update();
    }
    public override void StateEnter()
    {
        p.SetNextAnim("Walking");
    }
}