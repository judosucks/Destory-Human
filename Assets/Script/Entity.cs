using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private PlayerState playerState;
    private Player player => GetComponent<Player>();
    private Enemy _enemy => GetComponent<Enemy>();
    public EnemyData enemyData;
    public PlayerData playerData;
    public CapsuleCollider2D cd;
    [Header("Collision info")] 
   
    public Transform attackCheck;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected Transform wallBackCheck;
    [SerializeField] protected Transform headCheck;
    [SerializeField]protected Transform ledgeCheck;
    [SerializeField] private Transform edgeParentChecker;
    [SerializeField] private Transform leftGroundCheck;
    [SerializeField] private Transform rightGroundCheck;
    
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
    public int facingDirection { get; private set; } = 1;
    protected bool facingRight = true;
    [Header("edge detect info")]
    public EdgeTriggerDetection leftEdgeTrigger;
    public EdgeTriggerDetection rightEdgeTrigger;
    public bool isNearLeftEdge => leftEdgeTrigger && leftEdgeTrigger.isNearLeftEdge;
    public bool isNearRightEdge => rightEdgeTrigger && rightEdgeTrigger.isNearRightEdge;
    public bool IsFacingRight()
    {
        return facingRight;
    }
    [Header("edge detect info")]
    [SerializeField]private Transform leftEdgeCheck;
    [SerializeField]private Transform rightEdgeCheck;
    private Vector3 leftEdgeOriginalPosition;
    private Vector3 rightEdgeOriginalPosition;
    [Header("fall settings")] 
    public float highFallThreshold = 5f;
    public float highFallSpeedThreshold = -10f;
    protected float startFallHeight;
    protected bool isFalling;
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
        // Store the original local positions of the edge checkers
        leftEdgeOriginalPosition = leftEdgeCheck.localPosition;
        rightEdgeOriginalPosition = rightEdgeCheck.localPosition;

        Debug.Log($"Initial Left Edge Position: {leftEdgeOriginalPosition}");
        Debug.Log($"Initial Right Edge Position: {rightEdgeOriginalPosition}");
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
   
    protected virtual IEnumerator HitKnockback()
    {
      
        isKnocked = true;
        
        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDirection, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }
    
   
    public void EnemySetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
        {
            return; //if knocked can not move
        }
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        if (IsEnemyGroundDetected()) FlipController(xVelocity);
    }

    public void EnemyZeroVelocity()
    {
        if (isKnocked)
        {
            return; //if knocked can not move
        }

        rb.linearVelocity = new Vector2(0f, 0f);
    }
    #region collision

    public virtual bool CheckIfTouchingHead()
    {
        return Physics2D.Raycast(headCheck.position, Vector2.up, playerData.headCheckDistance, playerData.whatIsGround);
    }
    public virtual bool CheckIfTouchingLedge()
    {
        bool check =Physics2D.Raycast(ledgeCheck.position, Vector2.right * facingDirection, playerData.ledgeCheckDistance, playerData.whatIsGround);
        
        return check;
    }
    

    public virtual bool IsGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, playerData.groundCheckDistance, playerData.whatIsGround);
        bool isGroundDetected = hit.collider != null;
    
        
        return isGroundDetected;
    }

    public virtual bool IsEnemyGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, enemyData.groundCheckDistance, enemyData.whatIsGround);
        bool isGroundDetected = hit.collider != null;
    
        
    
        return isGroundDetected;
    }
    public virtual bool isWallBackDetected()
    {
        bool check = Physics2D.Raycast(wallBackCheck.position, Vector2.right * -player.facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        
        return check;
    }

    public virtual bool isLeftGroundDetected()
    {
        bool check = Physics2D.Raycast(leftGroundCheck.position, Vector2.down , playerData.groundCheckDistance, playerData.whatIsGround);
        
        return check;
    }

    public virtual bool isRightGroundDetected()
    {
        bool check = Physics2D.Raycast(rightGroundCheck.position, Vector2.down, playerData.groundCheckDistance, playerData.whatIsGround);
        
        return check;
    }
    public virtual bool IsWallDetected()
    { 
        // 墙检测逻辑
       bool check = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
       
       return check;
    }
    public virtual bool IsEnemyWallDetected()
    { 
        // 墙检测逻辑
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, enemyData.wallCheckDistance, enemyData.whatIsGround);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + playerData.ledgeCheckDistance, ledgeCheck.position.y));
        Gizmos.DrawLine(leftGroundCheck.position,new Vector3(leftGroundCheck.position.x ,leftGroundCheck.position.y- playerData.groundCheckDistance));
        Gizmos.DrawLine(rightGroundCheck.position,new Vector3(rightGroundCheck.position.x,rightGroundCheck.position.y - playerData.groundCheckDistance));
        Gizmos.DrawLine(headCheck.position,new Vector3(headCheck.position.x,headCheck.position.y + playerData.headCheckDistance));
        Gizmos.DrawLine(wallBackCheck.position, new Vector3(wallBackCheck.position.x - playerData.wallCheckDistance, wallBackCheck.position.y));
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - playerData.groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + playerData.wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position,playerData.attackCheckRadius);
        
    }
    


    #endregion
    
    #region Flip

    
    
    
    public virtual void Flip()
    {
        // Flip the facing direction
        facingDirection *= -1;
        facingRight = !facingRight;
        // transform.Rotate(0,180,0);
        // Flip the parent of the edge checkers
        Vector3 localScale = edgeParentChecker.localScale;
        localScale.x *= -1; // Flip the X-axis
        edgeParentChecker.localScale = localScale;

        // Flip the model by inverting its scale (if needed, for the player itself)
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        // // Flip the player's X-axis
        transform.localScale = playerScale;
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
