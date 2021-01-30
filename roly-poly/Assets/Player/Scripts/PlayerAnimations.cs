using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimations : MonoBehaviour
{
    public Animator anim;
    public int flipSprite;
    private Vector3 modelScale;
    private Quaternion modelRotation;
    void Start()
    {
        modelScale = transform.localScale;
        modelRotation = transform.localRotation;
    }
    public void Flip(int dir)
    {
        transform.localScale = Vector3.Scale(modelScale, new Vector3(dir * flipSprite, 1, 1));
    }
    public void Roll()
    {
        anim.SetTrigger("roll");
    }

    public void FlatIdle()
    {
        anim.SetTrigger("flatIdle");
    }

    public void FlatWalk()
    {
        anim.SetTrigger("flatWalk");
    }

    public void ResetRotation()
    {
        transform.localRotation = modelRotation;
    }
}
