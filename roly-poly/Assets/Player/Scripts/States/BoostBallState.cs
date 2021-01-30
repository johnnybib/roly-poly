using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBallState : PlayerState
{
    public BoostBallState(PlayerController p, BoostBall boostBall) : base(p, StateID.BoostBall) {
        this.boostBall = boostBall;   
    }
    private BoostBall boostBall;
    private float chargeDuration = 0;
    public override PlayerState HandleInput()
    {
        if(p.inputs.releaseBoostBall)
        {
            Boost();
            return new RollingState(p);
        }
        return null;
    }

    public override PlayerState Update()
    {
        chargeDuration += Time.fixedDeltaTime;
        if(chargeDuration >= boostBall.maxChargeTime)
        {
            Boost();
            return new RollingState(p);
        }
        return null;
    }

    public void Boost()
    {
        // Debug.Log("boost ball");
        // Debug.Log(chargeDuration);
        p.physics.BoostBall(chargeDuration * boostBall.forcePerSecond);
    }
    public override void StateEnter()
    {
        chargeDuration = 0;
    }
}
