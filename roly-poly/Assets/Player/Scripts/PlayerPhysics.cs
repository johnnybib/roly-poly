using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D flatCollider;
    public CircleCollider2D rollCollider;
    public PhysicsMaterial2D rollMat;
    public PhysicsMaterial2D walkMat;
    public float xAccel;
    public float stopThreshold;
    public float walkFriction;
    public float rollFriction;
    public float maxMoveSpeedWalk;
    public float maxMoveSpeedRoll;
    public float switchBumpAmount;
    
    public float raycastDist;
    
    
    [SerializeField]
    private bool isRoll;
    private int facingDir;
    private bool isFalling;
    private bool isGrounded;
    private int GROUND_LAYER_MASK;
    private Quaternion ecbRotation;


    void Awake()
    {
        GROUND_LAYER_MASK = 1 << LayerMask.NameToLayer("Ground");
        facingDir = -1;
    }

    void Start()
    {
        flatCollider.enabled = !isRoll;
        rollCollider.enabled = isRoll;
        ecbRotation = transform.localRotation;
    }
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDist, GROUND_LAYER_MASK);
        if(hit.collider != null) {
            Debug.DrawRay(transform.position, Vector2.down * raycastDist, Color.red);
            isGrounded = true;
        }
        else {
            Debug.DrawRay(transform.position, Vector2.down * raycastDist, Color.yellow);
            isGrounded = false;
        }
        if(isGrounded)
        {
            if(isRoll)
                rb.AddForce(Vector2.right * -Mathf.Sign(rb.velocity.x) * rollFriction * Time.fixedDeltaTime);
            else
                rb.AddForce(Vector2.right * -Mathf.Sign(rb.velocity.x) * walkFriction * Time.fixedDeltaTime);
            if(Mathf.Abs(rb.velocity.magnitude) < stopThreshold) {
                Stop();
            }
        }
        isFalling = rb.velocity.y < 0;

    }
    public void Move(float dir) 
    {
        
        rb.AddForce(Vector2.right * dir * xAccel * Time.fixedDeltaTime);
        if(isGrounded)
        {
            if(isRoll && Mathf.Abs(rb.velocity.magnitude) > maxMoveSpeedRoll){
                rb.velocity = rb.velocity.normalized * maxMoveSpeedRoll;
            }
            else if(!isRoll && Mathf.Abs(rb.velocity.magnitude) > maxMoveSpeedWalk){
                rb.velocity = rb.velocity.normalized * maxMoveSpeedWalk;
            }
        }
    }

    public void Roll(float dir)
    {
        rb.AddForce(Vector2.right * dir * xAccel * Time.fixedDeltaTime);
        if(isGrounded && Mathf.Abs(rb.velocity.magnitude) > maxMoveSpeedRoll)
            rb.velocity = rb.velocity.normalized * maxMoveSpeedRoll;
        
    }

    public void Walk(float dir)
    {
        rb.AddForce(transform.right * dir * xAccel * Time.fixedDeltaTime);
        if(isGrounded && Mathf.Abs(rb.velocity.magnitude) > maxMoveSpeedWalk)
            rb.velocity = rb.velocity.normalized * maxMoveSpeedWalk;
    }

    public void Bump(Vector2 dir, float force)
    {
        rb.AddForce(dir * force);
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    public void ToggleRoll()
    {
        isRoll = !isRoll;
        flatCollider.enabled = !isRoll;
        rollCollider.enabled = isRoll;
        rb.sharedMaterial = isRoll ? rollMat : walkMat;
        if(isGrounded)
        {
            Bump(Vector2.up, switchBumpAmount);
        }
    }

    public bool IsRoll()
    {
        return isRoll;
    }

    public void FlipFacingDirection()
    {
        facingDir = facingDir * -1;
    }
    public int GetFacingDir()
    {
        return facingDir;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
    public bool IsFalling()
    {
        return isFalling;
    }
    
    public void ResetRotation()
    {
        transform.localRotation = ecbRotation;
    }


    #region Ability Physics 
    public void Dribble(float force)
    {
        rb.AddForce(Vector2.down * force);
    }
    #endregion


}