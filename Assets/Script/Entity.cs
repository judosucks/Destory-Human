using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private PlayerState playerState;
    public CapsuleCollider2D cd;
    [Header("Collision info")] 
    public float attackCheckRadius;
    public Transform attackCheck;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform wallBackCheck;
    [SerializeField] protected float wallBackCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected float ledgeCheckDistance;
    [SerializeField]protected Transform ledgeCheck;
    
    [Header("air infor")]
    [SerializeField] protected float gravityMultiplier;
    public float GetGravityMultiplier()=> gravityMultiplier;
    [SerializeField]protected float maxFallSpeed;
    public float GetMaxFallSpeed()=> maxFallSpeed;
    [SerializeField] protected float fallForce;
    public float GetFallForce()=> fallForce;

    [Header("wallslide info")] 
    [SerializeField] protected float wallSlideDownForce;
    public float GetWallSlideDownForce()=> wallSlideDownForce;
    [SerializeField] protected float climbUpForce;
    public float GetClimbUpForce()=> climbUpForce;
    [Header("kneekick info")]
    public float kneeKickCooldown = 1.5f;
    public float kneeKickKnockbackForce = 10f;
    public bool isKneeKick;
    public Vector2 kneeKickKnockbackDirection;
    
    [Header("knockback info")] 
    [SerializeField] protected Vector2 knockbackDirection;

    protected bool isKnocked;
    [SerializeField] protected float knockbackDuration;
    
    [Header("cross kick info")]
    public Vector2 firstKickKnockbackForce;
    
    public float firstKickKnockbackYdirection;
   
    public bool isCrossKick;
    
    public float specialKnockbackForce = 10.0f; // 示例值，根据需要调整
    public float regularForce = 5.0f; // 示例值，根据需要调整
    public float regularForceY;
    [Header("ledge info")]
    public Vector2 startOffset;
    public Vector2 stopOffset;
    
    public bool isHanging;
    
    

    public bool isClimbLedge;
    public int facingDirection { get; private set; } = 1;
    protected bool facingRight = true;
    [Header("throw grenade info")]
    public bool isThrowComplete;
    [Header("sprint info")]
    public bool isSprint;

    [Header("blackhole info")] 
    [Header("grenade info")]
    public bool isAiming;

    
    public bool isAimCheckDecided;
    public bool rightButtonLocked;
    public bool grenadeCanceled;
    public bool mouseButttonIsInUse;
    
    #region components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    
    public SpriteRenderer sr{get;private set;}
    
    public CharacterStats stats { get; private set; }
    #endregion

    public System.Action onFlipped;
    protected virtual void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
        
    }

    protected virtual void Start()
    {
       
    }

    protected virtual void Update()
    {
        
    }

    public virtual void DamageEffect()=>StartCoroutine("HitKnockback");

    public virtual void SlowEntityBy(float _slowPercent, float _slowDuration)
    {
        
    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1f;
    }
    public void MakeTransparent(bool _transparent)
    {
        Color color = sr.color;
        color.a = _transparent ? 0f : 1.0f; // Adjust alpha for transparency
        sr.color = color;
        // if (_transparent)
        // {
        //     Debug.Log("make transparent true");
        //     sr.color = Color.clear;
        // }
        // else
        // {
        //     Debug.Log("make transparent false");
        //     sr.color = Color.white;
        // }
    }
    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDirection, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }
    
    #region velocity

    public void ZeroVelocity()
    {
        if (isKnocked)
        {
            return; //if knocked can not move
        }
        rb.linearVelocity = new Vector2(0f, 0f);
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
        {
            return; //if knocked can not move
        }
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        if (IsGroundDetected()) FlipController(xVelocity);
    }

    #endregion
    #region collision

    public virtual bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * facingDirection, ledgeCheckDistance, whatIsGround);
    }
    // public virtual bool IsGroundDetected()
    // {
    //     return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    // }

    public virtual bool IsGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        bool isGroundDetected = hit.collider != null;
    
        
    
        return isGroundDetected;
    }

    public virtual bool isWallBackDetected()
    {
        return Physics2D.Raycast(wallBackCheck.position, Vector2.left, wallBackCheckDistance, whatIsGround);
    }

    // public virtual bool IsWallDetected()
    // {
    //     return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection,
    //         wallCheckDistance, whatIsGround);
    // }
    public virtual bool IsWallDetected()
    {
        // 确保只有在玩家朝向墙的位置上才检测墙壁
        
        if (facingDirection != PlayerManager.instance.player.stateMachine.currentState.GetXDirection())
        {
            return false; // 如果玩家背对墙，则忽略墙的检测
        }

        // 墙检测逻辑
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + ledgeCheckDistance, ledgeCheck.position.y));
        
        
        Gizmos.DrawLine(wallBackCheck.position, new Vector3(wallBackCheck.position.x + wallBackCheckDistance, wallBackCheck.position.y));
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position,attackCheckRadius);
        
    }


    #endregion
    
    #region Flip

    
    public virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        
        if (onFlipped != null)
        {
          onFlipped();
        }

    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight) Flip();
    }
    #endregion

    public virtual void Die()
    {
        
    }
}
