using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingState : ActionableState
{
    private float currHorz;
    public RollingState(PlayerController p, StateID stateID = StateID.Null) : base(p, stateID != StateID.Null ? stateID : StateID.Rolling) { }

    public override PlayerState Update()
    {

        if (p.IsInputHorz())
        {
            p.physics.Roll(p.inputs.horz);
        }
        if(Mathf.Abs(p.physics.rb.velocity.x) > 0.1f)
        {
            p.CheckFlip(Mathf.Sign(p.physics.rb.velocity.x));
            p.animations.SetRollSpeed(p.physics.rb.velocity.magnitude * p.physics.GetFacingDir());
        }
        else 
        {
            p.animations.SetRollSpeed(0);
        }
        return base.Update();
    }
    public override void StateEnter()
    {
        p.SetNextAnim("Rolling");
    }

}
