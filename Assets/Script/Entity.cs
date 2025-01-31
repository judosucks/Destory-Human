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
    public bool isGroundDetected { get; private set; }
    public bool leftGroundDetected { get; private set; }
    public bool rightGroundDetected { get; private set; }
    public bool isWallBottomDetected { get; private set; }
    public bool isFrontBottomCheck { get; private set; }
    public bool isBottomGroundDetected { get; private set; }
    public bool isTopWallDetected { get; private set; }
    public bool isEdgeGroundDetected { get; private set; }
    public bool isLedgeDetected { get; private set; }
  
    public Transform attackCheck;
    [SerializeField] private Transform edgeGroundCheck;
    [SerializeField] private Transform topWallCheck;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected Transform wallBackCheck;
    [SerializeField] protected Transform headCheck;
    [SerializeField]protected Transform ledgeCheck;
    [SerializeField] private Transform edgeParentChecker;
    [SerializeField] private Transform leftGroundCheck;
    [SerializeField] private Transform rightGroundCheck;
    [SerializeField] private Transform wallCheckBottom;
    [SerializeField] private Transform frontBottomCheck;
    [SerializeField] private Transform bottomGroundCheck;
    [SerializeField] protected Transform ledgeCheckTwo;
    [SerializeField] protected Transform ledgeCheckTwo2;
    [Header("kneekick info")]
    public float kneeKickCooldown = 1.5f;
    public float kneeKickKnockbackForce = 10f;
    public bool isKneeKick;
    public Vector2 kneeKickKnockbackDirection;
    private AnimatorStateInfo stateInfo;
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
    public float startFallHeight;
    public float midFallThreshold = 3f;
    public float midFallSpeedThreshold = -7f;
    protected bool isFalling;
    public bool isHighFalling;
    public bool isMidFalling;
    
    public bool IsFalling()
    {
        return isFalling;
    }
    public bool isFallingFromEdge;
    public bool isFallingFromJump;
    #region components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    
    public SpriteRenderer sr{get;private set;}
    public AnimatorOverrideController overrideController; 
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
    
    public void PlayAnimationReversed(Animator _anim, int layerIndex, string _animName)
    {
        if (_anim == null)
        {
            Debug.LogWarning("Animator is null. Cannot play reversed animation.");
            return;
        }

        // Get the current state information
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(layerIndex);

        // Check if the requested animation is playing
        if (!stateInfo.IsName(_animName))
        {
            Debug.LogWarning("Animation state not found or not currently playing: " + _animName);
            return;
        }

        // Pause the animator so you can manually control the time progression
        _anim.speed = 0;

        // Calculate the reverse time based on the animation's length and current normalized time
        float reverseTime = stateInfo.length - (stateInfo.normalizedTime % 1) * stateInfo.length;

        // Play the animation again from the reverse time
        _anim.Play(_animName, layerIndex, reverseTime / stateInfo.length);

        Debug.Log("Animation reversed successfully: " + _animName);
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
        
        bool check =Physics2D.Raycast(ledgeCheck.position, Vector2.right * player.facingDirection, playerData.ledgeCheckDistance, playerData.whatIsGround);
        
        return check;
    }

    
    public virtual bool CheckIfTouchingLedgeTwo()
    {
        RaycastHit2D hit = Physics2D.Raycast(ledgeCheckTwo.position, Vector2.down, playerData.ledgeCheckDistance, playerData.whatIsGround);
        isLedgeDetected = hit.collider != null;
        return isLedgeDetected;
    }
    public virtual bool IsGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, playerData.groundCheckDistance, playerData.whatIsGround);
        isGroundDetected = hit.collider != null;
    
        
        return isGroundDetected;
    }

    public virtual bool IsEdgeGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(edgeGroundCheck.position,Vector2.down,playerData.edgeGroundDistance,playerData.whatIsGround);
        isEdgeGroundDetected = hit.collider != null;
        return isEdgeGroundDetected;
    }

    public virtual bool IsWallTopDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(topWallCheck.position, Vector2.right * facingDirection, playerData.wallTopCheckDistance, playerData.whatIsGround);
        isTopWallDetected = hit.collider != null;
        return isTopWallDetected;
    }
    public virtual bool IsBottomGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(bottomGroundCheck.position, Vector2.down, playerData.bottomGroundCheckDistance, playerData.whatIsGround);
        isBottomGroundDetected = hit.collider != null;
        return isBottomGroundDetected;
    }
    public virtual bool IsFrontBottomDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(frontBottomCheck.position, Vector2.right * facingDirection, playerData.frontBottomCheckDistance, playerData.whatIsGround);
        isFrontBottomCheck = hit.collider != null;
        return isFrontBottomCheck;
    }
    public virtual bool IsEnemyGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, enemyData.groundCheckDistance, enemyData.whatIsGround);
        bool isGroundDetected = hit.collider != null;
    
        
    
        return isGroundDetected;
    }
    public virtual bool isWallBackDetected()
    {
        bool check = Physics2D.Raycast(wallBackCheck.position, Vector2.right * -player.facingDirection, playerData.wallBackCheckDistance, playerData.whatIsGround);
        
        return check;
    }

    public virtual bool IsLeftGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(leftGroundCheck.position, Vector2.down , playerData.groundCheckDistance, playerData.whatIsGround);
        leftGroundDetected = hit.collider != null;
        return leftGroundDetected;
    }

    public virtual bool IsRightGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(rightGroundCheck.position, Vector2.down, playerData.groundCheckDistance, playerData.whatIsGround);
        rightGroundDetected = hit.collider != null;
        return rightGroundDetected;
    }
    public virtual bool IsWallDetected()
    { 
        // 墙检测逻辑
       bool check = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
       
       return check;
    } 
    public virtual bool IsWallBottomDetected()
    { 
        // 墙检测逻辑
        RaycastHit2D hit = Physics2D.Raycast(wallCheckBottom.position, Vector2.right * facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        isWallBottomDetected = hit.collider != null;
        return isWallBottomDetected;
    }
    
    public virtual bool IsEnemyWallDetected()
    { 
        // 墙检测逻辑
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, enemyData.wallCheckDistance, enemyData.whatIsGround);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(ledgeCheckTwo2.position, new Vector3(ledgeCheckTwo2.position.x, ledgeCheckTwo2.position.y-playerData.ledgeCheckDistance));
        Gizmos.DrawLine(edgeGroundCheck.position, new Vector3(edgeGroundCheck.position.x, edgeGroundCheck.position.y - playerData.edgeGroundDistance));
        Gizmos.DrawLine(topWallCheck.position, new Vector3(topWallCheck.position.x + playerData.wallTopCheckDistance, topWallCheck.position.y));
        Gizmos.DrawLine(bottomGroundCheck.position,new Vector3(bottomGroundCheck.position.x,bottomGroundCheck.position.y - playerData.bottomGroundCheckDistance));
        Gizmos.DrawLine(frontBottomCheck.position,new Vector3(frontBottomCheck.position.x + playerData.frontBottomCheckDistance,frontBottomCheck.position.y));
        Gizmos.DrawLine(wallCheckBottom.position, new Vector3(wallCheckBottom.position.x + playerData.wallCheckDistance, wallCheckBottom.position.y));
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + playerData.ledgeCheckDistance, ledgeCheck.position.y ));
        Gizmos.DrawLine(leftGroundCheck.position,new Vector3(leftGroundCheck.position.x ,leftGroundCheck.position.y- playerData.groundCheckDistance));
        Gizmos.DrawLine(rightGroundCheck.position,new Vector3(rightGroundCheck.position.x,rightGroundCheck.position.y - playerData.groundCheckDistance));
        Gizmos.DrawLine(headCheck.position,new Vector3(headCheck.position.x,headCheck.position.y + playerData.headCheckDistance));
        Gizmos.DrawLine(wallBackCheck.position, new Vector3(wallBackCheck.position.x - playerData.wallBackCheckDistance, wallBackCheck.position.y));
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
