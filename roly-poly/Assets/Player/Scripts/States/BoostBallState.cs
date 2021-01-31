using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBallState : PlayerState
{
    public BoostBallState(PlayerController p, BoostBall boostBall) : base(p, StateID.BoostBall)
    {
        this.boostBall = boostBall;
    }
    private BoostBall boostBall;
    private float chargeDuration = 0;
    public override PlayerState HandleInput()
    {
        p.CheckFlip(p.inputs.horz);
        if (p.inputs.releaseBoostBall)
        {
            Boost();
            return new RollingState(p);
        }
        return null;
    }

    public override PlayerState Update()
    {
        chargeDuration += Time.fixedDeltaTime;
        if (chargeDuration >= boostBall.maxChargeTime)
        {

            Boost();
            return new RollingState(p);
        }
        return null;
    }

    public void Boost()
    {
        if (GlobalSFX.Instance)
        {
            GlobalSFX.Instance.PlayBoostBallRelease();
        }
        // Debug.Log("boost ball");
        // Debug.Log(chargeDuration);
        p.physics.BoostBall(chargeDuration * boostBall.forcePerSecond, boostBall.duration);
    }

    public override void StateExit()
    {
        // p.physics.EnableGravity();
        base.StateExit();
    }
    public override void StateEnter()
    {
        // p.physics.Stop();
        // p.physics.DisableGravity();
        chargeDuration = 0;
        if (GlobalSFX.Instance)
        {
            //GlobalSFX.Instance.PlayBoostBallCharge();
        }
    }
}
