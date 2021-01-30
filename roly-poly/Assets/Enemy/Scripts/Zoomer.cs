using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoomer : Enemy
{
    public float raycastDist;
    public Transform raycastPointL;
    public Transform raycastPointR;

    public float moveSpeed;
    public float rotationSpeed;
    public int moveDir;
    private int GROUND_LAYER_MASK;
    private bool rotating;
    void Awake()
    {
        GROUND_LAYER_MASK = 1 << LayerMask.NameToLayer("Ground");
    }
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(GetRaycastPoint(),-transform.up, raycastDist, GROUND_LAYER_MASK);
        if(hit.collider != null) 
        {
            transform.Translate(Vector2.right * moveDir * moveSpeed * Time.fixedDeltaTime);   
            if(transform.localEulerAngles.z % 90 != 0)
            {
                transform.localEulerAngles = new Vector3(0, 0, Mathf.Round(transform.localEulerAngles.z/90)*90);
                Debug.Log("Set rotation to " + transform.localEulerAngles);
            }
            Debug.DrawRay(GetRaycastPoint(), -transform.up * raycastDist, Color.red);
            rotating = false;
        }
        else //rotate
        {
            rotating = true;
            Debug.DrawRay(GetRaycastPoint(), -transform.up * raycastDist, Color.yellow);
            transform.RotateAround(GetRotationPoint(), Vector3.forward, Time.fixedDeltaTime * rotationSpeed);
        }

    }

    private Vector3 GetRaycastPoint()
    {
        if(rotating)
        {
            if(moveDir < 0)
                return raycastPointL.position;
            else
                return raycastPointR.position;
        }
        else
        {
            if(moveDir < 0)
                return raycastPointR.position;
            else
                return raycastPointL.position;
        }
    }

    private Vector3 GetRotationPoint()
    {
        if(moveDir < 0)
            return raycastPointR.position;
        else
            return raycastPointL.position;
    }
}
