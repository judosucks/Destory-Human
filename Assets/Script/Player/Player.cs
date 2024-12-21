using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    private Vector2 workSpace;
    [Header("Attack Details")] 
    public Vector2[] attackMovement;

    public float counterAttackDuration = .2f;
    // [Header("cross kick")]
    // public float crossKickForce;

    
    
    [Header("Movement")] 
    public float movementSpeed = 2f;

    public float horizontalSpeed = 1f;
    public float straightJumpForce = 5f;
    public float jumpForce = 3f;
    public float grenadeReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("dash")] 
   
    private float defaultDashSpeed;
    public float dashSpeed;
    public float dashDuration;

    public SkillManager skill { get; private set; }
    public GameObject grenade { get; private set; }
    public PlayerInput playerInput;
    private bool isBusy; // 私有字段

    public bool GetIsBusy() // 公开方法获取属性
    {
        return isBusy;
    }

    public void SetIsBusy(bool value) // 公开方法设置属性
    {
        isBusy = value;
    }
    public bool isAttacking { get; set; } // 公开属性，用于指示玩家当前是否处于攻击状态

    
    public PlayerStateMachine stateMachine { get; private set; }
    
    public PlayerState playerState { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerStraightJumpState straightJumpState { get; private set; }
    public PlayerStraightJumpAirState straightJumpAirState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }

    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }

    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerCrossKickState crossKickState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerSprintState sprintState { get; private set; }
    public PlayerLedgeClimbState ledgeClimbState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerThrowGrenadeState throwGrenadeState { get; private set; }
    
    public PlayerBlackholeState blackholeState { get; private set; }
   
    public PlayerKneeKickState kneeKickState { get; private set; }
    
    public PlayerDeadState deadState { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        playerInput = GetComponent<PlayerInput>();
        if (anim == null)
        {
            Debug.LogError("Animator component is missing in children.");
        }
        sprintState = new PlayerSprintState(this, stateMachine, "Sprint");
        crossKickState = new PlayerCrossKickState(this, stateMachine, "CrossKick");
        straightJumpState = new PlayerStraightJumpState(this, stateMachine, "Jump");
        straightJumpAirState = new PlayerStraightJumpAirState(this, stateMachine, "Jump");
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "RunJump");
        airState = new PlayerAirState(this, stateMachine, "RunJump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "RunJump");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        ledgeClimbState = new PlayerLedgeClimbState(this,stateMachine,"LedgeClimbState");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        kneeKickState = new PlayerKneeKickState(this, stateMachine, "KneeKick");
        throwGrenadeState = new PlayerThrowGrenadeState(this, stateMachine, "AimGrenade");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Blackhole");
        deadState = new PlayerDeadState(this, stateMachine, "Dead");
    }

    protected override void Start()
    {
        EnsureSkillManagerIsInitialized(); // 确保初始化
        base.Start();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);
        defaultJumpForce = jumpForce;
        defaultMoveSpeed = movementSpeed;
        defaultDashSpeed = dashSpeed;
        if (SkillManager.instance == null)
        {
            Debug.LogError("SkillManager is null");
            return;
        }
        skill = SkillManager.instance;

        if (skill.dashSkill == null)
        {
            Debug.LogError("dashSkill is not initialized in SkillManager.");
            return;
        }
    }
    private void EnsureSkillManagerIsInitialized()
    {
        if (SkillManager.instance == null)
        {
            SkillManager.instance = Object.FindFirstObjectByType<SkillManager>();
            if (SkillManager.instance == null)
            {
                Debug.LogError("SkillManager instance could not be initialized.");
            }
        }
    }
    protected override void Update()
    {
        
        
        stateMachine?.currentState?.Update();
        if (isBusy)
        { 
            return; // 如果玩家处于忙碌状态，禁止其他输入
        } 
        DashInput(); // 冲刺输入处理
    }

    public override void SlowEntityBy(float _slowPercent, float _slowDuration)
    {
        movementSpeed = movementSpeed * (1 - _slowPercent);
        dashSpeed = dashSpeed * (1 - _slowPercent);
        jumpForce = jumpForce * (1 - _slowPercent);
        anim.speed = anim.speed * (1 - _slowPercent);
        Invoke("ReturnDefaultSpeed", _slowDuration);
        
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        movementSpeed = defaultMoveSpeed;
        dashSpeed = defaultDashSpeed;
        jumpForce = defaultJumpForce;
        
    }
    public void AssignNewGrenade(GameObject _newGrenade)
    {
        if (grenade)
        {
            Debug.LogWarning("Grenade already assigned! Destroying the new one.");
            Destroy(_newGrenade);
            return;
        }

        Debug.Log("Assigning new grenade...");
        grenade = _newGrenade;
    }

    public void ClearGrenade()
    {
        if (grenade == null) {
            Debug.LogWarning("No grenade to clear.");
            return;
        }

        Debug.Log("[ClearGrenade] Destroying grenade after delay...");
        StartCoroutine(DestroyGrenadeAfterDelay(.1f));
    }

    private IEnumerator DestroyGrenadeAfterDelay(float delay)
    {
        if (grenade == null) {
            Debug.LogWarning("[DestroyGrenadeAfterDelay] No grenade to destroy.");
            yield break;
        }

        yield return new WaitForSeconds(delay);

        if (grenade != null) {
            Debug.Log("[DestroyGrenadeAfterDelay] Destroying grenade...");
            Destroy(grenade);
            grenade = null;
        }
    }

    public void CancelThrowGrenade()
    {
        Debug.Log("cancelthrowgrenade and setbool false");
        isAiming = false;
        SkillManager.instance.grenadeSkill.DotsActive(false);
        Destroy(grenade);
        anim.SetBool("AimGrenade",false);
        CinemachineVirtualCamera currentCam = CameraManager.instance.GetCurrentActiveCamera();
        if (currentCam == null)
        {
            Debug.LogError("currentCam is null");
            return;
        }
        CameraManager.instance.newCamera.FollowPlayer(currentCam);

    }

    
    

    
    // public virtual void CheckForLedge()
    // {
    //     if(ledgeDetected && canGrabLedge)
    //     {
    //         canGrabLedge = false;
    //         Vector2 ledgePosition = GetComponentInChildren<PlayerLedgeCheckState>().transform.position;
    //         
    //         climbBegunPosition = ledgePosition + offset1;
    //         climbOverPosition = ledgePosition + offset2;
    //         canClimb = true;
    //         stateMachine.ChangeState(ledgeClimbState);
    //     }
    //
    //     if (canClimb)
    //     {
    //         transform.position = climbBegunPosition;
    //     }
    // }
    //
    
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        
        yield return new WaitForSeconds(_seconds);

        isBusy = false;
        
    }

   
    
    private void DashInput()
    {
       
        if (isBusy || IsWallDetected() || !IsGroundDetected())
        {
            
            return;
        }

       
          if ((Keyboard.current.fKey.wasPressedThisFrame && skill.dashSkill.CanUseSkill()) || (Gamepad.current != null &&
                 Gamepad.current.buttonEast.wasPressedThisFrame && skill.dashSkill.CanUseSkill()))
            {
            
                stateMachine.ChangeState(dashState);
            // // 如果不在冲刺状态，重置 canPerformDashAttack 为 false
            // if (!(stateMachine.currentState is PlayerDashState))
            // {
            //     playerState.canPerformDashAttack = false;
            // }
             } 
        
    }



    // public Vector2 DetermineCornerPosition()
    // {
    //     RaycastHit2D xHit = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    //     float xDistance = xHit.distance;
    //     workSpace.Set(xDistance * facingDirection, 0f);
    //     RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (vector3)(workSpace),Vector2.down,ledgeCheck.position.y - wallCheck.position.y, whatIsGround);
    //     float yDistance = yHit.distance;
    //     workSpace.Set(wallCheck.position.x + (xDistance * facingDirection),ledgeCheck.position.y - yDistance);
    //     return workSpace;
    // }
    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        float xDistance = xHit.distance;
        workSpace.Set(xDistance * facingDirection, 0f);

        // 将workSpace转换为Vector3
        Vector3 workspace3 = new Vector3(workSpace.x, workSpace.y, 0f);
    
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + workspace3, Vector2.down, ledgeCheck.position.y - wallCheck.position.y, whatIsGround);
        float yDistance = yHit.distance;
        workSpace.Set(wallCheck.position.x + (xDistance * facingDirection), ledgeCheck.position.y - yDistance);
        return workSpace;
    }
    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }
    public void OnDashAttackFrame()
    {
        
        stateMachine.currentState.CanPerformDashAttack();
    }
    public void OnDashAttackComplete()
    {
        stateMachine.currentState.CanNotPerformDashAttack();
    }

    public void OnPerformCrossKick()
    {
        
        stateMachine.currentState.PerformCrossKick();
    }

    public void OnCrossKickComplete()
    {
        
        stateMachine.currentState.PerformRegularAttack();
    }

    public bool OnAimingStart()
    {
        stateMachine.currentState.IsAimingGrenade();
        return true;
    }

    public bool OnAimingStop()
    {
        stateMachine.currentState.NotAimingGrenade();
        return true;
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
    // private Rigidbody2D rb;
    // private PlayerInput playerInput;
    // private InputAction jumpAction;
    // private InputAction DashAction;
    // [Header("Movement")]
    // public float moveSpeed = 2f;
    // // 最大跳跃力
    // public float maxJumpForce = 100f;
    // // 最小跳跃力
    // public float minJumpForce = 4f;
    // // 最大耐力
    // public float maxStamina = 100f;
    // // 每次跳跃消耗的耐力
    // public float staminaCostPerJump = 20f;
    // // 每秒恢复的耐力
    // public float staminaRecoveryRate = 10f;
    // // 跳跃所需的最小耐力
    // public float minStaminaToJump = 20f;
    // private bool isJumping = false;
    // private float currentStamina;
    // private bool canJump = true;
    //
    // float moveDirection;
    //
    // [Header("GroundCheck")]
    // public Transform groundCheckPos;
    // public LayerMask whatIsGround;
    // public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    // [Header("Gravity")]
    // public float baseGravity = 9.81f;
    // public float maxFallSpeed = 18f;
    // public float fallSpeedMultiplier = 2.5f;
    // [Header("Dash info")] 
    // public float staminaCostPerDash = 30f;
    //
    // public float minStaminaToDash = 30f;
    // [SerializeField] private float dashDuration;
    // [SerializeField] private float dashTime;
    // [SerializeField] private float dashSpeed = 20f;
    // private bool isDashing = false;
    // private bool canDash = true;
    //
    // private Animator anim;
    // public bool isMoving;
    // private int facingDirection = 1;
    // private bool facingRight = true;
    // public float currentSpeed;
    //
    // private void Awake()
    // {
    //     playerInput = GetComponent<PlayerInput>();
    //     rb = GetComponent<Rigidbody2D>();
    //     anim = GetComponentInChildren<Animator>();
    //     // 添加输入动作和我们的跳跃方法的绑定
    //     jumpAction = playerInput.actions["Jump"];
    //     jumpAction.started +=  OnJump;
    //     jumpAction.performed += OnJump;
    //     jumpAction.canceled +=  OnJump;
    //     DashAction = playerInput.actions["Dash"];
    //     DashAction.performed +=  OnDash;
    //     currentStamina = maxStamina;
    // }
    //
    //
    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     
    //     rb.linearVelocity = new Vector2(moveDirection* moveSpeed , rb.linearVelocity.y );
    //     
    //     isMoving = rb.linearVelocity.x != 0;
    //     anim.SetBool("isMoving",isMoving);
    //     anim.SetFloat("yVelocity", rb.linearVelocity.y);
    //     anim.SetBool("isGrounded",isGrounded());
    //     RecoverStamina();
    //     // Gravity();
    //     FlipController();
    //     dashTime -= Time.deltaTime;
    //     
    //     
    // }
    //
    // private void FixedUpdate()
    // {
    //     
    // }
    // public void Move(InputAction.CallbackContext context)
    // {
    //     moveDirection = context.ReadValue<Vector2>().x;
    // }
    //
    // private bool isGrounded()
    // {
    //     print("grounded");
    //     Collider2D groundCheck = 
    //         Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, whatIsGround);
    //     return groundCheck != null;
    //     
    // }
    // public void OnJump(InputAction.CallbackContext context)
    // {
    //     if (context.started && isGrounded() && canJump && currentStamina >= minStaminaToJump)
    //     {
    //         isJumping = true;
    //         
    //     }else if (context.canceled && isJumping)
    //     {
    //         // 判断按下时长以决定跳跃高度
    //         float baseJumpForce = (context.duration >= 0.1f) ? maxJumpForce : minJumpForce;
    //         float appliedJumpForce = AdjustJumpForceByStamina(baseJumpForce);
    //        // 跳跃并减少耐力
    //        // rb.linearVelocity = new Vector2(rb.linearVelocity.x, appliedJumpForce);
    //        rb.AddForce(Vector2.up * appliedJumpForce, ForceMode2D.Impulse);
    //        currentStamina -= staminaCostPerJump;
    //        CheckStaminaForActions();
    //        
    //        isJumping = false;
    //     } 
    // }
    // private float AdjustJumpForceByStamina(float baseForce)
    // {
    //     float staminaPercentage = currentStamina / maxStamina;
    //     if (staminaPercentage > 0.8f)
    //     {
    //         print("stamina full");
    //         // 最高跳跃力
    //         return baseForce;
    //     }else if (staminaPercentage < 0.6f)
    //     {
    //         // 耐力在60%-80%，跳跃力降低10%
    //         print("stamina 80");
    //         return baseForce * 0.6f;
    //     }else if (staminaPercentage > 0.4f)
    //     {
    //         // 耐力在40%-60%，跳跃力降低20%
    //         print("stamina 60");
    //         return baseForce * 0.4f;
    //     }else if (staminaPercentage > 0.2f)
    //     {
    //         // 耐力在20%-40%，跳跃力降低30%
    //         print("stamina 40");
    //         return baseForce * 0.2f;
    //     }
    //     else
    //     {
    //         // 耐力在0%-20%，跳跃力降低40%
    //         print("stamina 20");
    //         return baseForce * 0.1f;
    //     }
    // }
    // public void OnDash(InputAction.CallbackContext context)
    // {
    //     if (context.performed && canDash && currentStamina >= minStaminaToDash)
    //     {
    //         print("dash");
    //         dashTime = dashDuration;
    //         StartCoroutine(Dash());
    //
    //
    //     }
    // }
    //
    // private IEnumerator Dash()
    // {
    //    if(isDashing)
    //        yield break;
    //    
    //    
    //         // 开始冲刺
    //         isDashing = true;
    //         // 备份重力
    //         float originalGravity = rb.gravityScale;
    //         rb.gravityScale = 0f; // 暂时忽略重力
    //         rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed,0);
    //     
    //         currentStamina -= staminaCostPerDash;
    //         CheckStaminaForActions();
    //
    //         yield return new WaitForSeconds(dashTime);
    //         
    //         rb.gravityScale = originalGravity;// 恢复重力
    //         // rb.linearVelocity = Vector2.zero;
    //         isDashing = false;
    //         print("isdashing");
    //     
    //     
    // }
    //
    //
    // private void RecoverStamina()
    // {
    //     if (!isJumping && !isDashing && currentStamina < maxStamina)
    //     {
    //         currentStamina += staminaRecoveryRate * Time.deltaTime;
    //         CheckStaminaForActions();
    //         print(currentStamina);
    //         
    //     }
    // }
    //
    // private void CheckStaminaForActions()
    // {
    //     canJump = currentStamina >= minStaminaToJump;
    //     canDash = currentStamina >= minStaminaToDash;
    // }
    // private void Gravity()
    // {
    //     if (rb.linearVelocity.y < 0)
    //     {
    //         rb.gravityScale = baseGravity * fallSpeedMultiplier;
    //         rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y,-maxFallSpeed));
    //     }
    //     else
    //     {
    //         rb.gravityScale = baseGravity;
    //     }
    // }
    //
    // private void Flip()
    // {
    //     facingDirection = facingDirection * -1;
    //     facingRight = !facingRight;
    //     transform.Rotate(0f, 180f, 0f);
    // }
    //
    // private void FlipController()
    // {
    //     if (rb.linearVelocity.x > 0 && !facingRight)
    //     {
    //         Flip();
    //     }else if (rb.linearVelocity.x < 0 && facingRight)
    //     {
    //         Flip();
    //     }
    // }
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    // }
}