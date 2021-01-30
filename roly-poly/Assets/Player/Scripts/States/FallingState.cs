using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : AerialState
{
    public FallingState(PlayerController p) : base(p, StateID.Falling) { }
    public override void StateEnter()
    {
        // p.SetNextAnim("Falling");
        base.StateEnter();
    }

    public override void StateExit()
    {
        if(p.physics.IsGrounded())
        {
            // p.fxManager.PlayLandDust();
            // p.playerAudioController.PlayLand();
        }
        base.StateExit();
    }

}
