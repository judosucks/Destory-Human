using Unity.VisualScripting;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private bool isTouchingLedge;
    private bool isTouchingWall;
    private bool isTouchingGround;
    private bool isWallBackDetected;
    private new int xInput;
    private bool isTouchingHead;
    private bool isTouchingRightEdge;
    private bool isTouchingLeftEdge;
    private bool isTouchingGroundBottom;
    private bool isTouchingWallBottom;
    private bool isWallTopDetected;
    private bool isEdgeGrounded;
    public bool oldIsTouchingGround;
    private float fallTime = 0f;
    private bool isTouchingLedgeTwo;
    private bool grabInput;
    private bool jumpInput;
    private bool wallJumpCoyoteTime;
    private float startWallJumpCoyoteTime;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool coyoteTime;
    private bool isJumping;
    private bool jumpInputStop;
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData,
        string _animBoolName) : base(_player,
        _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        fallTime = 0f;
        playerData.isInAir = true;
        player.startFallHeight = 0f;
        player.startFallHeight = player.transform.position.y;
        // 仅对垂直速度进行重置
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);

        


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }


    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingLedge = LedgeTriggerDetection.isTouchingLedge;
        isTouchingWall = player.IsWallDetected();
        isTouchingGround = player.IsGroundDetected();
        oldIsTouchingGround = isTouchingGround;
        isWallBackDetected = player.isWallBackDetected();
        isTouchingHead = player.CheckIfTouchingHead();
        
        isTouchingLeftEdge = player.isNearLeftEdge;
        isTouchingRightEdge = player.isNearRightEdge;
        isTouchingGroundBottom = player.IsBottomGroundDetected();
        isTouchingWallBottom = player.IsWallBottomDetected();
        isWallTopDetected = player.IsWallTopDetected();
        isEdgeGrounded = player.IsEdgeGroundDetected();
        isTouchingLedgeTwo = player.CheckIfTouchingLedgeTwo();
        isTouchingWallBottom = player.IsWallBottomDetected();
        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isWallBackDetected;
        if (!wallJumpCoyoteTime && !isTouchingWall && !isWallBackDetected &&
            (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
        if (isTouchingLedge&& !playerData.isClimbLedgeState)
        {
            Debug.Log("ledgeposition");
            player.ledgeClimbState.SetDetectedPosition(LedgeTriggerDetection.ledgePosition);
           
        }
    }

    public override void Exit()
    {
        base.Exit();
        playerData.isInAir = false;
        isTouchingWall = false;
        isTouchingLedge = false;
        isWallBackDetected = false;
        isTouchingHead = false;
        playerData.reachedApex = false;
        isTouchingRightEdge = false;
        isTouchingLeftEdge = false;
        isTouchingGroundBottom = false;
        isTouchingWallBottom = false;
        isWallTopDetected = false;
        isEdgeGrounded = false;
        isTouchingGround = false;
        oldIsTouchingGround = false;
        isTouchingWallBottom = false;
        isTouchingLedgeTwo = false;
        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        
    }   


    public override void Update()
    {
        base.Update();
        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();
        xInput = player.inputController.norInputX;
        grabInput = player.inputController.grabInput;
        jumpInput = player.inputController.runJumpInput;
        jumpInputStop = player.inputController.jumpInputStop;
        CheckJumpMutiplier();
        

        if (isTouchingGround && Mathf.Approximately(rb.linearVelocity.y, 0f))
        {
            fallTime = 0f;
            float fallDistance = player.startFallHeight - player.transform.position.y;

            // 优化空中到地面状态的切换条件
            if (fallDistance >= player.highFallThreshold)
                stateMachine.ChangeState(player.highFallLandState);
            else if (fallDistance >= player.midFallThreshold)
                stateMachine.ChangeState(player.fallLandState);
            else
                stateMachine.ChangeState(player.runJumpLandState);
        }
        else if (!isTouchingGround && !isTouchingWall || !isTouchingGround && !isTouchingWall && isWallBackDetected)
        {
            // 强制保持空中状态，无其他干扰 // 全面控制空中行为
            fallTime += Time.deltaTime;
            float gravity = playerData.gravityMultiplier * Time.deltaTime;
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x, // 保持水平速度不变
                rb.linearVelocity.y + Physics2D.gravity.y * gravity
            );
            player.CheckIfShouldFlip(xInput); // 检查翻转
            player.SetVelocityX(playerData.airMovementSpeed * xInput);
            player.anim.SetFloat("yVelocity", rb.linearVelocity.y); // 更新动画
            player.anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));

        }
        else if (isTouchingLedge && !playerData.isClimbLedgeState&&!isTouchingGround)
        {
            Debug.Log("ledge");
            stateMachine.ChangeState(player.ledgeClimbState);
        }
        else if (jumpInput && (isTouchingWall || isWallBackDetected || wallJumpCoyoteTime))
        {   
            StopWallJumpCoyoteTime();
            isTouchingWall = player.IsWallDetected();
            player.wallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.wallJumpState);
        }
        else if (jumpInput && player.jumpState.CanJump())
        {
            Debug.LogWarning("jumping canjump");
            stateMachine.ChangeState(player.jumpState);
        }
       
        else if (isTouchingWall && grabInput)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
        else if (isTouchingWall && xInput == player.facingDirection && rb.linearVelocity.y <= 0f)
        {
            playerData.reachedApex = false;
            playerData.isWallSliding = true;
            stateMachine.ChangeState(player.wallSlideState);
        }
        
       
        if (player.isFallingFromEdge)
        {
            Debug.LogWarning("fallingfromedge");
            player.MoveTowardSmooth(playerData.moveDirection, playerData.moveDistance);
            player.isFallingFromEdge = false;
        }
        
    }

    private void CheckJumpMutiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            { 
                Debug.LogWarning("jumpjump");
                player.SetVelocityY(rb.linearVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;    
            }
            else if (rb.linearVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.jumpState.DecrementAmountOfJumpsLeft();
        }
    }
    public void StartCoyoteTime()=> coyoteTime = true;
    private void CheckWallJumpCoyoteTime()
    {
        if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
        {
            wallJumpCoyoteTime = false;
        }
    }
    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;
    public void SetIsJumping()=>isJumping = true;
}