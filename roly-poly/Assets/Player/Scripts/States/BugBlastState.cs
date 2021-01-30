using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugBlastState : WalkingState
{
    public BugBlastState(PlayerController p, BugBlast bugBlast) : base(p, StateID.BugBlast) {
        this.bugBlast = bugBlast;   
    }
    private BugBlast bugBlast;

    public override PlayerState Update()
    {
        return new WalkingState(p);
    }
    public override void StateEnter()
    {
        p.physics.BugBlast(bugBlast.blastForce);
    }
}
