using Unity.VisualScripting;
using UnityEngine;

public class PlayerStraightJumpAirState : PlayerState
{
    private bool isTouchingLedge;
    private bool isTouchingWall;
    private bool isTouchingGround;
    private bool isWallBackDetected;
    private bool isTouchingHead;
    private int xInput;
    private bool isDetecting;
    private bool isTouchingGroundBottom;
    private bool isTouchingWallBottom;
    private bool isWallTopDetected;
    private bool isEdgeGrounded;
    
    public PlayerStraightJumpAirState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
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
            Debug.Log("reachedApex");
        }

        // 墙相关的状态切换逻辑
        if (isTouchingWall && !isTouchingLedge&& isDetecting&& !isTouchingGroundBottom)
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
        // 跟踪墙和ledge状态
        isTouchingLedge = player.CheckIfTouchingLedge();
        isTouchingWall = player.IsWallDetected();
        isTouchingGround = player.IsGroundDetected();
        isWallBackDetected = player.isWallBackDetected();
        isTouchingHead = player.CheckIfTouchingHead();
        isTouchingGroundBottom = player.IsBottomGroundDetected();
        isTouchingWallBottom = player.IsWallBottomDetected();
        isWallTopDetected = player.IsWallTopDetected();
        xInput = player.inputController.norInputX;
        isEdgeGrounded = player.IsEdgeGroundDetected();
        // if (player.inputController.isJumping)
        // {
        //     playerData.highestPoint = player.transform.position.y;
        // }

        if (isTouchingWall && !isTouchingLedge&& !isTouchingGroundBottom)
        {
            
              isDetecting = true;
              player.ledgeClimbState.SetDetectedPosition(player.transform.position);
            
        }
    }

    public override void Enter()
    {
        base.Enter();
        
        playerData.isInAir = true;
        
    }

    public override void Exit()
    {
        base.Exit();
        playerData.isInAir = false;
        isTouchingGround = false;
        isTouchingWall = false;
        isTouchingLedge = false;
        isWallBackDetected = false;
        playerData.reachedApex = false;
        isTouchingHead = false;
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
       

        if (isWallTopDetected && isTouchingWallBottom )
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
                        Debug.Log("hieight" + playerData.reachedApex);
                        playerData.highestPoint = player.transform.position.y;
                    }

                    if (player.transform.position.y < playerData.highestPoint && !playerData.reachedApex && rb.linearVelocity.y <= 0f)
                    {
                        
                        playerData.reachedApex = true;
                        
                        Debug.Log("reached highest point"+" "+playerData.highestPoint);
                        if (rb.linearVelocity.y < 0f)
                        {
                            Debug.Log("falling down" + player.CurrentVelocity.y);
                            Debug.Log("reached false");
                            
                            player.isFallingFromJump = true;
                            player.ApplyGravityAndClampVelocity();
                            
                        }
                    }
                
            }
            
        }
        
        // else if (!isTouchingGround && xInput != 0)
        // {
        //     player.SetVelocityX(xInput * playerData.movementSpeed * .8f);ss
        // }

        if (isTouchingGround && rb.linearVelocity.y <0.01f )
        {
            playerData.reachedApex = false;
            float fallDistance = player.startFallHeight - player.transform.position.y; //calculate fall distance
            if (fallDistance >= player.highFallThreshold || rb.linearVelocity.y < player.highFallSpeedThreshold)
            {
                //Debug.Log("fall distance is greater than highFallThreshold");
                stateMachine.ChangeState(player.hurtState);
                return;
            }

            if (fallDistance >= player.midFallThreshold || rb.linearVelocity.y < player.midFallSpeedThreshold)
            {
                stateMachine.ChangeState(player.fallLandState);
                return;
            }
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.runJumpLandState);
                return;
            }
            
              stateMachine.ChangeState(player.straightJumpLandState);
             

          
            
        }

        
        
        if (isTouchingHead)
        {
            Debug.Log("touching head");
            player.StopUpwardVelocity();
        }
       
    }
}
