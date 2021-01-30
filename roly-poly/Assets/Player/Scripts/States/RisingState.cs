using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingState : AerialState
{
    public RisingState(PlayerController p) : base(p, StateID.Rising) { }

    public override void StateEnter()
    {
        // p.SetNextAnim("Rising");
        base.StateEnter();
    }

}
