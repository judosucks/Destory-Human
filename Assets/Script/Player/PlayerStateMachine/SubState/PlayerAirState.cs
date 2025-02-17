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

        


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }


    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingLedge = player.CheckIfTouchingLedge();
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
        if (isTouchingWall && !isTouchingLedge&&!playerData.isClimbLedgeState)
        {
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
           
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
        
        if (isTouchingGround && rb.linearVelocity.y < 0.01f)
        {
            fallTime = 0f;
            
            float fallDistance = player.startFallHeight - player.transform.position.y; //calculate fall distance
            
            if (fallDistance >= player.highFallThreshold)
            {
                Debug.LogWarning("high"); 
                stateMachine.ChangeState(player.highFallLandState);
            }
            else if(fallDistance >= player.midFallThreshold)
            {
                Debug.LogWarning("isfirst true fall");
                stateMachine.ChangeState(player.fallLandState);
            }
            else
            {
                stateMachine.ChangeState(player.runJumpLandState);
            }
        } 
        else if (isTouchingWall && !isTouchingLedge&& !playerData.isClimbLedgeState&&!isTouchingGround)
        {
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
       
        else if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
        else if (isTouchingWall && xInput == player.facingDirection && rb.linearVelocity.y <= 0f)
        {
            playerData.reachedApex = false;
            playerData.isWallSliding = true;
            stateMachine.ChangeState(player.wallSlideState);
        }
        
        
        else 
        {
            
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(playerData.movementSpeed  * xInput);
            player.anim.SetFloat("yVelocity",rb.linearVelocity.y);
            player.anim.SetFloat("xVelocity",Mathf.Abs(rb.linearVelocity.x));
        }

        // bool isJumping = player.anim.GetBool("RunJump");
        // if (isJumping && rb.linearVelocity.y > 0f)
        // {
        //     if (isTouchingLeftEdge&&!isTouchingRightEdge&&!player.IsFrontBottomDetected() || isTouchingRightEdge && !isTouchingLeftEdge&&!player.IsFrontBottomDetected())
        //     {
        //         Debug.LogWarning("standstate");
        //         stateMachine.ChangeState(player.standState);
        //     }
        // }

        if (!isTouchingGround && rb.linearVelocity.y < 0f)
        {
            fallTime += Time.deltaTime;
            
            
            player.ApplyFallingGravity(playerData.gravityMultiplier);
           
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