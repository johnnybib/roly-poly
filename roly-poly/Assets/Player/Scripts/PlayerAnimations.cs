using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimations : MonoBehaviour
{
    public Animator anim;
    public int flipSprite;
    public float rollSpeedMultiplier;
    private Vector3 modelScale;
    private Quaternion modelRotation;
    void Start()
    {
        modelScale = transform.localScale;
        modelRotation = transform.localRotation;
    }

    public void Play(string name)
    {
        anim.SetTrigger(name);
    }
    public void Flip(int dir)
    {
        transform.localScale = Vector3.Scale(modelScale, new Vector3(dir * flipSprite, 1, 1));
    }

    public void ResetRotation()
    {
        transform.localRotation = modelRotation;
    }

    public void SetRollSpeed(float speed)
    {
        anim.SetFloat("Speed", -Mathf.Abs(speed) * rollSpeedMultiplier);
    }
    public void ClearAnimTriggers()
    {
        // Debug.LogWarning("EntityController: ResetStates() -> No animations implemented. Cannot reset states.");
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                anim.ResetTrigger(param.name);
            }
        }
    }
}
