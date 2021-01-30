using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DribbleState : AerialState
{
    public DribbleState(PlayerController p, Dribble dribble) : base(p, StateID.Dribble) {
        this.dribble = dribble;   
    }
    private Dribble dribble;
    public override void StateEnter()
    {
        p.physics.Dribble(dribble.dribbleForce);
        base.StateEnter();
    }

}
