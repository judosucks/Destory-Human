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
    private PlayerColliderManager playerColliderManager;

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
    public bool isEdgeDetected { get; private set; }
    public bool isEdgeWallDetected { get; private set; }
    public bool isTouchingCeiling { get; private set; }
    private bool touchingWall; // 是否接触墙
    public bool isTouchingWall => touchingWall; // 外部可访问的只读属性
    public Transform attackCheck;
    [SerializeField] private Transform ceilingTransform;
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
    [SerializeField] protected Transform edgeCheck;
    [SerializeField] protected Transform edgeWallCheck;
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
    public float lowFallThreshold = 1f;
    
    public bool isHighFalling;
    public bool isMidFalling;
    public bool isLowFalling;
    public bool isFallingFromEdge;
    public bool isFallingFromJump;
    [Header("slope info")]
    public Vector2 colliderSize { get;private set; }
    public Vector2 slopeNormalPerp { get; private set;}
    public bool isOnSlope { get; private set; } = false;
    private float slopeDownAngleOld;
    private float slopeDownAngle;
    private float slopeSideAngle;
    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;
    [SerializeField] private float maxSlopeAngle;
    public bool canWalkOnSlope { get;private set; }
    [Header("currentvelocity info")]
    
    private Vector2 workspace;
    private Vector2 workSpace2;
  
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
        playerColliderManager = GetComponent<PlayerColliderManager>();
        colliderSize = cd.size;
        
    }

    protected virtual void Start()
    {
        // Store the original local positions of the edge checkers
        leftEdgeOriginalPosition = leftEdgeCheck.localPosition;
        rightEdgeOriginalPosition = rightEdgeCheck.localPosition;
        
    }

    protected virtual void Update()
    {
        

        if (playerColliderManager != null)
        {
            Vector2 currentWorkspace = playerColliderManager.workspace;

            // 仅在 workspace 变化时更新
            if (currentWorkspace != workspace)
            {
                workspace = currentWorkspace;

             #if UNITY_EDITOR
                Debug.Log("Current Workspace Updated: " + workspace);
             #endif
                // 根据 workspace 处理逻辑
            }
        }
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
       #region velocity
   

    public void SnapToGridSize(float _gridSize)
    {
        Transform objTransform = GetComponent<Transform>();
        float snappedX = Mathf.Round(objTransform.position.x / _gridSize) * _gridSize;
        float snappedY = Mathf.Round(objTransform.position.y / _gridSize) * _gridSize;
        objTransform.position = new Vector2(snappedX, snappedY);
    }

    public void MoveTowardSmooth(Vector2 direction, float distance)
    {
        Transform objTransform = GetComponent<Transform>();
        Vector2 targetPosition = (Vector2)objTransform.position + direction.normalized * distance;
        objTransform.position = Vector2.Lerp(objTransform.position, targetPosition, Time.deltaTime * playerData.movementSpeed);
    }
    public void ApplyFallingGravity(float multiplier)
    {
        if (rb.linearVelocity.y < 0) // Only apply when falling
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (multiplier - 1) * Time.deltaTime;
        }
        
    }
    public void StopUpwardVelocity()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Min(rb.linearVelocity.y, 0f));
        
        
    }
    
    public void ZeroVelocity()
    {
        if (isKnocked)
        {
            return; //if knocked can not move
        }

        rb.linearVelocity = Vector2.zero;
        
    }
    
    public void SetVelocity(float velocity, Vector2 angle , int direction)
    {
        if (isKnocked)
        {
            return; //if knocked can not move
        }
        angle.Normalize();
        rb.linearVelocity = new Vector2(angle.x * velocity * direction, angle.y * velocity);
    }
    
    
    public void SetVelocityY(float yVelocity)
    {
        if (isKnocked)
        {
            return;
        }
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, yVelocity);
    }

    public void SetVelocityX(float xVelocity)
    {
        if (isKnocked)
        {
            return;
        }
        rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
        
        // if (IsGroundDetected()) FlipController(xVelocity);
        // if (!IsGroundDetected()) FlipController(xVelocity);
    }
    #endregion
    #region collision
    public void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, colliderSize.y / 2));

        // 检查水平坡度和垂直坡度
        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);

        // 重置逻辑：如果角度接近平坦，就认为不在坡地上
        if (Mathf.Approximately(slopeDownAngle, 0.0f) && Mathf.Approximately(slopeSideAngle, 0.0f))
        {
            isOnSlope = false; // 无坡自动重置
        }
    }
    public void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, playerData.slopeCheckDistance, playerData.whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, playerData.slopeCheckDistance, playerData.whatIsGround);

        if (slopeHitFront)
        {
            float slopeFAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up); // 检查前面的坡度
            if (slopeFAngle > 0.1f) // 如果坡度合法，定义为坡地
            {
                isOnSlope = true;
                slopeSideAngle = slopeFAngle;
            }
        }
        else if (slopeHitBack)
        {
            float slopeBAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up); // 检查后面的坡度
            if (slopeBAngle > 0.1f)
            {
                isOnSlope = true;
                slopeSideAngle = slopeBAngle;
            }
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false; // 无坡重置
        }

    }

    public void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, playerData.slopeCheckDistance,
            playerData.whatIsGround);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;            
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);
            Debug.LogWarning($"Slope Normal Perpendicular: {slopeNormalPerp}");
            Debug.LogWarning($"Slope Down Angle: {slopeDownAngle}");

            // 如果坡度角度大于0或发生了变化，判断为在坡地
            if (slopeDownAngle > 0.1f) // 小于0.1度认为在平面上
            {
                isOnSlope = true;
            }
            else
            {
                isOnSlope = false; // 如果太平坦（比如接近 0 度），重置坡地状态
            }

            slopeDownAngleOld = slopeDownAngle;

            // 画出调试线，用于检查坡度检测是否正确
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }
        else
        {
            slopeDownAngle = 0.0f;
            isOnSlope = false; // Raycast Miss，说明没有坡地
        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }
        Debug.LogWarning($"canWalkOnSlope: {canWalkOnSlope}");

        if (isOnSlope && player.inputController.norInputX == 0f && canWalkOnSlope)
        {
            Debug.LogWarning("Slope Check Vertical isonslope");
            rb.sharedMaterial = fullFriction;
        }
        else
        {
            rb.sharedMaterial = noFriction;
        }
    }
    public void CheckGroundStatus()
    {
        IsGroundDetected();  // 统一检测地面

        SlopeCheck(); // 检查坡地状态
    }
    public virtual bool CheckIfTouchingHead()
    {
        return Physics2D.Raycast(headCheck.position, Vector2.up, playerData.headCheckDistance, playerData.whatIsGround);
    }

    public virtual bool CheckIfTouchingCeiling()
    {
        RaycastHit2D hit = Physics2D.Raycast(ceilingTransform.position,Vector2.up,playerData.ceilingCheckDistance,playerData.whatIsGround);
        isTouchingCeiling = hit.collider != null;
        return isTouchingCeiling;
    }
    public virtual bool CheckIfTouchingLedge()
    {
        
        bool check =Physics2D.Raycast(ledgeCheck.position, Vector2.right * player.facingDirection, playerData.ledgeCheckDistance, playerData.whatIsGround);
        
        return check;
    }

    public virtual bool CheckIfTouchingEdge()
    {
        RaycastHit2D hit = Physics2D.Raycast(edgeCheck.position, Vector2.right * player.facingDirection, playerData.edgeCheckDistance, playerData.whatIsEdge);
        isEdgeDetected = hit.collider != null;
        return isEdgeDetected;
    }
    public virtual bool CheckIfTouchingEdgeWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(edgeWallCheck.position, Vector2.right * player.facingDirection, playerData.edgeCheckDistance, playerData.whatIsEdge);
        isEdgeWallDetected = hit.collider != null;
        return isEdgeWallDetected;
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
        Debug.Log($"IsGroundDetected returns: {isGroundDetected}");

        
        return isGroundDetected;
    }

    public virtual bool IsEdgeGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(edgeGroundCheck.position,Vector2.down,playerData.edgeGroundDistance,playerData.whatIsEdge);
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
        RaycastHit2D hit = Physics2D.Raycast(bottomGroundCheck.position, Vector2.down, playerData.bottomGroundCheckDistance, playerData.groundAndEdgeLayer);
        isBottomGroundDetected = hit.collider != null;
        return isBottomGroundDetected;
    }
    public virtual bool IsFrontBottomDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(frontBottomCheck.position, Vector2.right * facingDirection, playerData.frontBottomCheckDistance, playerData.groundAndEdgeLayer);
        isFrontBottomCheck = hit.collider != null;
        return isFrontBottomCheck;
    }
    public virtual bool IsEnemyGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, enemyData.groundCheckDistance, enemyData.groundAndEdgeLayer);
        bool isEnemyGroundDetected = hit.collider != null;
    
        
    
        return isEnemyGroundDetected;
    }
    public virtual bool isWallBackDetected()
    {
        bool check = Physics2D.Raycast(wallBackCheck.position, Vector2.right * -player.facingDirection, playerData.wallBackCheckDistance, playerData.whatIsGround);
        
        return check;
    }

    public virtual bool IsLeftGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(leftGroundCheck.position, Vector2.down , playerData.groundCheckDistance, playerData.groundAndEdgeLayer);
        leftGroundDetected = hit.collider != null;
        if (!leftGroundDetected)
        {
            Debug.Log("left ground not detected");
        }
        return leftGroundDetected;
    }

    public virtual bool IsRightGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(rightGroundCheck.position, Vector2.down, playerData.groundCheckDistance, playerData.groundAndEdgeLayer);
        rightGroundDetected = hit.collider != null;
        if (!rightGroundDetected)
        {
            Debug.Log("right ground not detected");
        }
        return rightGroundDetected;
    }
    public virtual bool IsWallDetected()
    {
       // 墙检测逻辑
       touchingWall = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
       
       return touchingWall;
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
        Gizmos.DrawLine(ceilingTransform.position, new Vector3(ceilingTransform.position.x, ceilingTransform.position.y + playerData.ceilingCheckDistance));
        Gizmos.DrawLine(edgeCheck.position, new Vector3(edgeCheck.position.x + playerData.edgeCheckDistance, edgeCheck.position.y));
        Gizmos.DrawLine(edgeWallCheck.position, new Vector3(edgeWallCheck.position.x + playerData.edgeCheckDistance, edgeWallCheck.position.y));
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
    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != facingDirection)
        {
            Flip();
            
        }
    }
    
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight) Flip();
        
       

    }
    #endregion

    #region otherFunction

    
    public Vector2 DetermineCornerPosition()
    {
        // 获取 CapsuleCollider2D 的半径
        float capsuleRadius = playerData.standColliderSize.x / 2f; // x 是水平半径

        // 水平方向的检测 (改动)
        RaycastHit2D xHit = Physics2D.Raycast(
            wallCheck.position, 
            Vector2.right * facingDirection, 
            playerData.wallCheckDistance + capsuleRadius,  // 增加半径补偿
            playerData.whatIsGround
        );
        float xDistance = xHit.distance - capsuleRadius;  // 减去圆弧半径
        Debug.Log("x distance: " + xDistance);
    
        // 更新 workspace 的起始位置 (保持水平补偿)
        workspace.Set(xDistance * facingDirection, 0);

        // 垂直方向的检测 (改动)
        RaycastHit2D yHit = Physics2D.Raycast(
            ledgeCheck.position + (Vector3)(workspace), 
            Vector2.down, 
            ledgeCheck.position.y - wallCheck.position.y + 0.015f,
            playerData.whatIsGround
        );

        float yDistance = yHit.distance;
        Debug.Log("y distance: " + yDistance);

        // 更新 workspace 的最终位置
        workspace.Set(
            wallCheck.position.x + (xDistance * facingDirection), 
            ledgeCheck.position.y - yDistance
        );
        Debug.Log("workspace is set to: " + workspace);

        return workspace;
    }
    public Vector2 DetermineEdgeCornerPosition()
    {
        // 从`edgeWallCheck`发射水平射线
        RaycastHit2D xHit = Physics2D.Raycast(edgeWallCheck.position, Vector2.right * facingDirection, playerData.edgeCheckDistance, playerData.whatIsEdge);
        float xDistance = xHit.distance;

        // 水平射线结果
        workSpace2.Set(xDistance * facingDirection, 0f);
    
        // 从`edgeCheck`位置发射垂直射线
        RaycastHit2D yHit = Physics2D.Raycast(edgeCheck.position + (Vector3)(workSpace2), Vector2.down, edgeCheck.position.y - edgeWallCheck.position.y, playerData.whatIsEdge);
        float yDistance = yHit.distance;

        // 返回精确的边缘Corner
        workSpace2.Set(edgeWallCheck.position.x + (xDistance * facingDirection), edgeCheck.position.y - yDistance);
        Debug.Log("workspace2"+workSpace2);
        return workSpace2;
    }
    #endregion
    public virtual void Die()
    {
        
    }
}