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
    public float maxSpeedGroundedWalk;
    public float maxSpeedGroundedRoll;
    public float maxSpeedAerialRoll;
    public float maxSpeedAerialWalk;
    public float switchBumpAmount;

    public float raycastDistRoll;
    public float raycastDistWalk;
    public float killSpeed;

    public Vector3 hitPointOffset;
    public float knocbackTime;
    [HideInInspector]
    public Vector2 prevVel;

    [SerializeField]
    private bool isRoll;

    private int facingDir;
    private bool isFalling;
    private bool isGrounded;
    private bool isKnockback;
    private bool isBoostball;
    private bool canKill;
    private int GROUND_LAYER_MASK;
    private float gravityScale;
    private Quaternion ecbRotation;


    void Awake()
    {
        GROUND_LAYER_MASK = 1 << LayerMask.NameToLayer("Ground");
        facingDir = -1;
        gravityScale = rb.gravityScale;
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
        // Debug.Log(rb.velocity.magnitude);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, IsRoll() ? raycastDistRoll : raycastDistWalk, GROUND_LAYER_MASK);
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, Vector2.down * (IsRoll() ? raycastDistRoll : raycastDistWalk), Color.red);
            if (!IsGrounded() && IsRoll() && rb.velocity.magnitude > 8f)
            {
                p.animations.PlayLandingParticles(hit.point);
                if (GlobalSFX.Instance)
                {
                    GlobalSFX.Instance.PlayHitGround();
                }
            }
            isGrounded = true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.down * (IsRoll() ? raycastDistRoll : raycastDistWalk), Color.yellow);
            isGrounded = false;
        }
        if (IsGrounded() && !isKnockback)
        {
            if (IsRoll())
                rb.AddForce(Vector2.right * -Mathf.Sign(rb.velocity.x) * rollFriction * Time.fixedDeltaTime);
            else
            {
                rb.AddForce(Vector2.right * -Mathf.Sign(rb.velocity.x) * walkFriction * Time.fixedDeltaTime);
            }

            if (Mathf.Abs(rb.velocity.magnitude) < stopThreshold)
            {
                StopX();
            }
        }
        isFalling = rb.velocity.y < 0;
        Debug.DrawLine(transform.position, transform.position + hitPointOffset, Color.green);
        prevVel = rb.velocity;
        if (prevVel.magnitude > killSpeed && IsRoll())
        {
            canKill = true;
            p.animations.RotateCanKill(Quaternion.LookRotation(rb.velocity.normalized) * Quaternion.FromToRotation(Vector3.right, Vector3.forward) * Quaternion.FromToRotation(Vector3.right, Vector3.up));
            p.animations.ShowCanKill();
        }
        else
        {
            canKill = false;
            p.animations.HideCanKill();
        }

    }
    public void Move(float dir)
    {
        rb.AddForce(Vector2.right * dir * GetXAccel() * Time.fixedDeltaTime);
        float maxMoveSpeed = GetMaxSpeed();
        if (!isKnockback && Mathf.Abs(rb.velocity.magnitude) > maxMoveSpeed)
            rb.velocity = rb.velocity.normalized * maxMoveSpeed;
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
        if (IsGrounded())
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
        if (IsGrounded())
        {
            if (IsRoll())
                return xAccelGroundedRoll;
            else
                return xAccelGroundedWalk;
        }
        else
        {
            if (IsRoll())
                return xAccelAerialRoll;
            else
                return xAccelAerialWalk;
        }
    }

    public float GetMaxSpeed()
    {
        if(isBoostball){
            return Mathf.Infinity;
        }
        if (IsGrounded())
        {
            if (IsRoll())
                return maxSpeedGroundedRoll;
            else
                return maxSpeedGroundedWalk;
        }
        else
        {
            if (IsRoll())
                return maxSpeedAerialRoll;
            else
                return maxSpeedAerialWalk;
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

    public bool CanKill()
    {
        return canKill;
    }

    public void EnableGravity()
    {
        rb.gravityScale = gravityScale;
    }
    public void DisableGravity()
    {
        rb.gravityScale = 0;
    }

    #region Ability Physics 
    public void Dribble(float force)
    {
        rb.AddForce(Vector2.down * force);
    }

    public void BoostBall(float boostForce, float duration)
    {
        rb.AddForce(Vector2.right * boostForce * GetFacingDir());

        StartCoroutine(BoostballTimer(duration));
    }

    private IEnumerator BoostballTimer(float time)
    {
        isBoostball = true;
        yield return new WaitForSeconds(time);

        isBoostball = false;
    }


    public void BugBlast(Vector2 dir, float blastForce)
    {
        if (GlobalSFX.Instance)
        {
            GlobalSFX.Instance.PlayBugBlast();
        }
        if (IsGrounded() && dir.x != 0)
            dir.x = 0;
        if (dir == Vector2.zero)
            dir = Vector2.up;
        rb.AddForce(dir * blastForce);
    }
    #endregion


}