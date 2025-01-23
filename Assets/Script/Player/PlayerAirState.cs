using UnityEngine;

public class PlayerAirState : PlayerState
{
    private bool isTouchingLedge;
    private bool isTouchingWall;
    private bool isTouchingGround;
    private bool isWallBackDetected;
    private int xInput;
    private bool isTouchingHead;
    private bool isTouchingRightEdge;
    private bool isTouchingLeftEdge;
    private bool isDetecting;
    private bool isTouchingGroundBottom;
    private bool isTouchingWallBottom;  
    private bool isWallTopDetected;
    private bool isEdgeGrounded;
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        playerData.isInAir = true;

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        
        if (playerData.reachedApex  &&!isTouchingGround&& player.IsRightGroundDetected()&& !player.IsLeftGroundDetected() && !isTouchingWall || playerData.reachedApex &&   player.IsLeftGroundDetected()&& !player.IsRightGroundDetected()&& !isTouchingWall &&!isTouchingGround)
        {
            if (Mathf.RoundToInt(xDirection) != player.facingDirection && Mathf.RoundToInt(xDirection)!= 0)
            {
                playerData.reachedApex = false;
                return;
            }

            Debug.LogWarning("entering reachedapex");
            if (!player.IsFrontBottomDetected())
            {
                Debug.LogWarning("stand IsFrontBottomDetected()");
                playerData.reachedApex = false;
                player.MoveTowardSmooth(playerData.moveDirection * player.facingDirection,playerData.moveDistance);   
                stateMachine.ChangeState(player.standState);
                if (player.IsGroundDetected())
                {
                    Debug.LogWarning("stand IsGroundDetected()");
                    player.SnapToGridSize(playerData.gridSize);
                    rb.AddForce(Vector2.down * playerData.stickingForce, ForceMode2D.Impulse);
                }
                
            }
           

        }
        if (player.CurrentVelocity.y < -5f || player.IsLeftGroundDetected() && !player.IsRightGroundDetected()&&!isTouchingGround || !isTouchingGround &&
            !player.IsLeftGroundDetected() && player.IsRightGroundDetected())
        {
            playerData.reachedApex = false;
            Debug.Log("reachedApex"+player.CurrentVelocity.y);
        }
       
        // 如果触碰到墙但不是ledge，并且玩家还在下降态势时
        // Refine ledge detection; ensure ledge transitions are prioritized
        if (isTouchingWall && !isTouchingLedge && isDetecting && !isTouchingGroundBottom)
        {
              isDetecting = false;
              stateMachine.ChangeState(player.ledgeClimbState);
            
        }

        if (isWallBackDetected && player.isFallingFromEdge || isWallBackDetected && player.isFallingFromJump)
        {
            Debug.Log("wall back detected");
            player.isFallingFromEdge = false;
            player.MoveTowardSmooth(playerData.moveDirection * player.facingDirection,playerData.moveDistance);
        }
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingLedge = player.CheckIfTouchingLedge();
        isTouchingWall = player.IsWallDetected();
        isTouchingGround = player.IsGroundDetected();
        isWallBackDetected = player.isWallBackDetected();
        isTouchingHead = player.CheckIfTouchingHead();
        xInput = player.inputController.norInputX;
        isTouchingLeftEdge = player.isNearLeftEdge;
        isTouchingRightEdge = player.isNearRightEdge;
        isTouchingGroundBottom = player.IsBottomGroundDetected();
        isTouchingWallBottom = player.IsWallBottomDetected();
        isWallTopDetected = player.IsWallTopDetected();
        isEdgeGrounded = player.IsEdgeGroundDetected();
        if (isTouchingWall && !isTouchingLedge && !isTouchingGroundBottom)
        {
              isDetecting = true;
              player.ledgeClimbState.SetDetectedPosition(player.transform.position);
            
        }
    }

    public override void Exit()
    {
        base.Exit();
       playerData.isInAir = false;
       isTouchingGround = false;
       isTouchingWall = false;
       isTouchingLedge = false;
       isWallBackDetected = false;
       isTouchingHead = false;
       playerData.reachedApex = false;
       isTouchingRightEdge = false;
       isTouchingLeftEdge = false;
       isDetecting = false;
       isTouchingGroundBottom = false;
       isTouchingWallBottom = false;
       isWallTopDetected = false;
       isEdgeGrounded = false;

    }

    
    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected())
        {
            player.CheckIfFallingDown();
        }


        

        if (xInput != 0)
        {
            player.SetVelocity(playerData.movementSpeed * .6f * xInput, rb.linearVelocity.y);
        }
         if (isTouchingHead)
         {
             Debug.Log("touching head");
             player.StopUpwardVelocity();
         }
        

// Ensure wall slide only happens when actively sliding down a wall
        if (isWallTopDetected && player.IsWallBottomDetected())
        {
            playerData.reachedApex = false;
            playerData.isWallSliding = true;
            stateMachine.ChangeState(player.wallSlideState);
        }
        
        if (!isTouchingWall && !isTouchingGround && rb.linearVelocity.y <= 0)
        {
            if (!player.inputController.isJumping)
            {
                Debug.Log("not jumping");
                player.isFallingFromEdge = true;
                
                player.ApplyGravityAndClampVelocity();
                
                //state machine change to falling state when animation work is done
            }
            else if (player.inputController.isJumping)
            {
                Debug.Log("jumping");
               
                player.isFallingFromEdge = false;
                player.isFallingFromJump = false;
               
                if (player.transform.position.y > playerData.highestPoint)
                {
                    Debug.Log("hieight");
                    playerData.highestPoint = player.transform.position.y;
                }

                if (player.transform.position.y < playerData.highestPoint && !playerData.reachedApex && rb.linearVelocity.y <= 0f)
                {
                    
                   
                    playerData.reachedApex = true;
                   
                    Debug.Log("reached highest point"+" "+playerData.highestPoint);
                    if (rb.linearVelocity.y < 0f)
                    {
                        Debug.Log("falling down" + player.CurrentVelocity.y);
                        
                        playerData.reachedApex = false;
                        player.isFallingFromJump = true;
                        player.ApplyGravityAndClampVelocity();
                      
                    }
                }
                
            }
            
        }
       
        // else if (!isTouchingGround && xInput != 0)
        // {
        //     player.SetVelocityX(xInput *playerData.movementSpeed * .8f);
        // }

      
        
      

        if (isTouchingGround && rb.linearVelocity.y <0.01f )
        {
            playerData.reachedApex = false;

            if (player.isHighFalling)
            {
                player.isHighFalling = false;
                stateMachine.ChangeState(player.hurtState);
                return;
            }

            if (player.isMidFalling)
            {
                player.isMidFalling = false;
                stateMachine.ChangeState(player.fallLandState);
                return;
            }
            stateMachine.ChangeState(player.runJumpLandState);
        }

        
        
        
        
    }

   
}