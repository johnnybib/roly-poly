using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public PlayerPhysics physics;
    public PlayerAnimations animations;
    public GameObject model;




    //Use fixed update because the ecb moves in fixed update
    void FixedUpdate()
    {
        model.transform.position = physics.rb.transform.position;   
        if(!physics.isRoll) 
            model.transform.localRotation = physics.rb.transform.localRotation;
    }

    public void OnHorizontal(Vector2 dir)
    {
        physics.Move(dir);
        animations.Flip(physics.GetFacingDir());
        // if(!physics.isRoll)
        // {
        //     if(dir == Vector2.zero)
        //         animations.FlatIdle();
        //     else
        //         animations.FlatWalk();
        // }
    }

    public void OnSwitchMode()
    {
        physics.ToggleRoll();
        physics.Stop();
        animations.ResetRotation();
        if(physics.isRoll) {
            animations.Roll();
        }
        else {
            animations.FlatWalk();
            physics.ResetRotation();
        }
    }
}
