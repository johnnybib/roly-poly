using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingState : ActionableState
{
    private float currHorz;
    public RollingState(PlayerController p) : base(p, StateID.Rolling) { }

    public override PlayerState Update()
    {
        if (p.IsInputHorz())
        {
            p.physics.Roll(p.inputs.horz);
        }
        p.animations.SetRollSpeed(p.physics.rb.velocity.magnitude * p.physics.GetFacingDir());
        return base.Update();
    }
    public override void StateEnter()
    {
        p.SetNextAnim("Rolling");
    }

}
