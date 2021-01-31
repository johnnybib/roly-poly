using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionableState : PlayerState
{
    public ActionableState(PlayerController p, StateID stateID) : base(p, stateID) { }
    public override PlayerState HandleInput()
    {
        if (p.inputs.switchMode)
        {
            if (GlobalSFX.Instance)
            {
                GlobalSFX.Instance.PlaySwitchMode();
            }
            p.animations.ResetRotation();
            p.physics.ToggleRoll();
            p.physics.StopX();
            if (!p.physics.IsRoll())
            {
                p.physics.ResetRotation();
                if (p.IsInputHorz())
                {
                    return new WalkingState(p);
                }
                else
                {
                    return new IdleState(p);
                }
            }
            else
            {

                return new RollingState(p);
            }
        }

        return p.abilities.CheckAbilities();
    }

    public override PlayerState Update()
    {
        return null;
    }


}
