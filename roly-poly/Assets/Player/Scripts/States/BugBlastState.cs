using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugBlastState : WalkingState
{
    public BugBlastState(PlayerController p, BugBlast bugBlast) : base(p, StateID.BugBlast)
    {
        this.bugBlast = bugBlast;
        timer = 0;
    }
    private BugBlast bugBlast;
    private float timer;

    public override PlayerState Update()
    {
        timer += Time.fixedDeltaTime;
        if (timer > bugBlast.duration)
        {
            p.animations.ResetRotation();
            p.physics.ToggleRoll();
            return new WalkingState(p);
        }
        return null;
    }

    public override void StateExit()
    {
        p.physics.EnableGravity();
        base.StateExit();
    }
    public override void StateEnter()
    {
        p.animations.Play("BugBlast");
        p.physics.Stop();
        p.physics.BugBlast(new Vector2(p.inputs.horz, p.inputs.vert), bugBlast.blastForce);
        if (Mathf.Abs(p.inputs.horz) > 0.5)
            p.animations.PlayBugBlastParticles(Quaternion.LookRotation(new Vector2(p.inputs.horz, p.inputs.vert)) * Quaternion.FromToRotation(Vector3.forward, Vector3.right));
        else
            p.animations.PlayBugBlastParticles(Quaternion.LookRotation(new Vector2(p.inputs.horz, p.inputs.vert)) * Quaternion.FromToRotation(Vector3.up, Vector3.right));
        // GameObject.Destroy(GameObject.Instantiate(bugBlast.bugBlastHitbox, p.physics.transform.position + p.physics.hitPointOffset, Quaternion.identity), 0.1f);
        p.physics.DisableGravity();
    }
}
