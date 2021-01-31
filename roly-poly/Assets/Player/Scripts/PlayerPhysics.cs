using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
 
    [HideInInspector]
    public PlayerController p;
    public Rigidbody2D rb;
    public BoxCollider2D flatCollider;
    public CircleCollider2D rollCollider;
    public PhysicsMaterial2D rollMat;
    public PhysicsMaterial2D walkMat;
    public float xAccelAerialRoll;
    public float xAccelGroundedRoll;
    public float xAccelAerialWalk;
    public float xAccelGroundedWalk;
    public float stopThreshold;
    public float walkFriction;
    public float rollFriction;
    public float maxMoveSpeedWalk;
    public float maxMoveSpeedRoll;
    public float switchBumpAmount;
    
    public float raycastDistRoll;
    public float raycastDistWalk;
    
    public Vector3 hitPointOffset;
    public float knocbackTime;

    
    [SerializeField]
    private bool isRoll;

    private int facingDir;
    private bool isFalling;
    private bool isGrounded;
    private bool isKnockback;
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
    public void SetPlayerController(PlayerController p)
    {
        this.p = p;
    }
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, IsRoll() ?  raycastDistRoll : raycastDistWalk, GROUND_LAYER_MASK);
        if(hit.collider != null) {
            Debug.DrawRay(transform.position, Vector2.down * ( IsRoll() ?  raycastDistRoll : raycastDistWalk), Color.red);
            isGrounded = true;
        }
        else {
            Debug.DrawRay(transform.position, Vector2.down * ( IsRoll() ?  raycastDistRoll : raycastDistWalk), Color.yellow);
            isGrounded = false;
        }
        if(IsGrounded() && !isKnockback)
        {
            if(isRoll)
                rb.AddForce(Vector2.right * -Mathf.Sign(rb.velocity.x) * rollFriction * Time.fixedDeltaTime);
            else
                rb.AddForce(Vector2.right * -Mathf.Sign(rb.velocity.x) * walkFriction * Time.fixedDeltaTime);
            if(Mathf.Abs(rb.velocity.magnitude) < stopThreshold) {
                StopX();
            }
        }
        isFalling = rb.velocity.y < 0;
        Debug.DrawLine(transform.position, transform.position + hitPointOffset, Color.green);

    }
    public void Roll(float dir)
    {
        rb.AddForce(Vector2.right * dir * GetXAccel() * Time.fixedDeltaTime);
        if(IsGrounded() && !isKnockback && Mathf.Abs(rb.velocity.magnitude) > maxMoveSpeedRoll)
            rb.velocity = rb.velocity.normalized * maxMoveSpeedRoll;
        
    }

    public void Walk(float dir)
    {
        rb.AddForce(transform.right * dir * GetXAccel() * Time.fixedDeltaTime);
        if(IsGrounded() && !isKnockback && Mathf.Abs(rb.velocity.magnitude) > maxMoveSpeedWalk)
            rb.velocity = rb.velocity.normalized * maxMoveSpeedWalk;
    }

    public void Bump(Vector2 dir, float force)
    {
        rb.AddForce(dir * force);
    }


    public void Knockback(Vector2 dir, float force)
    {
        rb.AddForce(dir * force);

        StartCoroutine(KnockbackTimer(knocbackTime));
    }

    private IEnumerator KnockbackTimer(float time)
    {
        isKnockback = true;
        yield return new WaitForSeconds(time);

        isKnockback = false;   
    }


    public void StopX()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.angularVelocity = 0;
    }
    public void StopY()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    public void Stop()
    {
        StopX();
        StopY();
    }

    public void ToggleRoll()
    {
        isRoll = !isRoll;
        flatCollider.enabled = !isRoll;
        rollCollider.enabled = isRoll;
        rb.sharedMaterial = isRoll ? rollMat : walkMat;
        if(IsGrounded())
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

    public float GetXAccel()
    {
        if(IsGrounded())
        {
            if(IsRoll())
                return xAccelGroundedRoll;
            else
                return xAccelGroundedWalk;
        }
        else
        {
            if(IsRoll())
                return xAccelAerialRoll;
            else
                return xAccelAerialWalk;
        }
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

    public void BoostBall(float boostForce)
    {       
        rb.AddForce(Vector2.right * boostForce * GetFacingDir());
    }

    public void BugBlast(float blastForce)
    {
        rb.AddForce(Vector2.up * blastForce);
    }
    #endregion


}