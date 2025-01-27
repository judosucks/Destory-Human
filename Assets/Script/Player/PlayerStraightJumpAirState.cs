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
    public bool oldIsTouchingGround { get;private set; }
    private bool isFirstLand;
    private bool trigger;
    private float fallTime = 0f;
    private bool isTouchingLedgeTwo;
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
               
                
            }
        }
        if (player.CurrentVelocity.y < 0)
        {
            if (fallTime > 0.2f && player.isFallingFromJump)
            {
                playerData.reachedApex = false;    
            }
            
            
        }

        // 墙相关的状态切换逻辑
        if (isTouchingWall && !isTouchingLedgeTwo && !isTouchingGroundBottom&&isDetecting)
        {
            Debug.Log("rb"+rb.linearVelocity.x);
            if (rb.linearVelocity.x > -0.1f && player.facingDirection == -1&& rb.linearVelocity.y < 0 && rb.linearVelocity.y > -0.1 ||
                rb.linearVelocity.x < 0.1f && player.facingDirection == 1 && rb.linearVelocity.y < 0 && rb.linearVelocity.y > -0.1) ;
            {
                isDetecting = false;
                stateMachine.ChangeState(player.ledgeClimbState);
                
            }


        }
        if (isWallBackDetected && player.isFallingFromEdge || isWallBackDetected && player.isFallingFromJump)
        {
            player.isFallingFromEdge = false;
            player.MoveTowardSmooth(playerData.moveDirection * player.facingDirection,playerData.moveDistance);
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();
        // 跟踪墙和ledge状态
        isTouchingGroundBottom = player.IsBottomGroundDetected();
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
        oldIsTouchingGround = isTouchingGround;
        isTouchingLedgeTwo = player.CheckIfTouchingLedgeTwo();
        // if (player.inputController.isJumping)
        // {
        //     playerData.highestPoint = player.transform.position.y;
        // }

        if (isTouchingWall && !isTouchingLedgeTwo && !isTouchingGroundBottom)
        {
            
            if (rb.linearVelocity.x > -0.1f && player.facingDirection == -1&& rb.linearVelocity.y < 0 && rb.linearVelocity.y > -0.1 ||
                rb.linearVelocity.x < 0.1f && player.facingDirection == 1 && rb.linearVelocity.y < 0 && rb.linearVelocity.y > -0.1) ;
            {
                Debug.LogWarning("falltime"+fallTime+"rb"+rb.linearVelocity.x+"rb"+rb.linearVelocity.y);
                isDetecting = true;
                player.ledgeClimbState.SetDetectedPosition(player.transform.position);
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
        fallTime = 0f;
        playerData.isInAir = true;
        player.startFallHeight = 0f; 
        player.startFallHeight = player.transform.position.y;
        Debug.Log("player"+player.startFallHeight+"playerData.highestPoint"+player.startFallHeight);
        player.ApplyGravityAndClampVelocity();
        
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
        isFirstLand = false;
        oldIsTouchingGround = false;
        trigger = false;
        isTouchingLedgeTwo = false;
    }

    public override void Update()
    {
        base.Update();
        if (!isTouchingGround && rb.linearVelocity.y < 0)
        {
            fallTime += Time.deltaTime;
            
        }
        else
        {
          
            fallTime = 0f;
        }
        
        // 状态检查逻辑
        if (isTouchingGround && rb.linearVelocity.y < 0.01f)
        {
            float fallDistance = player.startFallHeight - player.transform.position.y; //calculate fall distance
            
           
            
            if (!isFirstLand && fallDistance >= player.highFallThreshold)
            {
                trigger = false; 
                isFirstLand = false;
                stateMachine.ChangeState(player.highFallLandState);
                return;
            }else
            {
              
              
                isFirstLand = true;
            }

            // 其次处理中等高度掉落
            
            if (isFirstLand&& fallDistance >= player.midFallThreshold)
            {

                trigger = false;
                isFirstLand = false;
             
                stateMachine.ChangeState(player.fallLandState);
                return;


            }
            else
            {
                trigger = true;
               
                
            }
            if (xInput != 0 && trigger)
            {
                isFirstLand = false;
                trigger = false;
                stateMachine.ChangeState(player.runJumpLandState);
                return;
            }
            
            stateMachine.ChangeState(player.straightJumpLandState);
            isFirstLand = false;
            trigger = false;
            
            return;

        }


     
        
        if (xInput != 0)
        {
            if (player.isFallingFromJump)
            {
            
              player.SetVelocity(playerData.movementSpeed * .6f * xInput, rb.linearVelocity.y);
            }

            if (player.isFallingFromEdge)
            {
               
                player.SetVelocity(playerData.movementSpeed * .2f * xInput, rb.linearVelocity.y);
            }
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
             
                player.isFallingFromEdge = true;
                playerData.reachedApex = false;
                player.ApplyGravityAndClampVelocity();
                if (player.isFallingFromJump)
                {
                   
                    player.isFallingFromEdge = false;
                }
                //state machine change to falling state when animation work is done
            }
            else if (player.inputController.isJumping)
                {
                
                   
                    player.isFallingFromEdge = false;
                    player.isFallingFromJump = false;
                    
                    if (player.transform.position.y > playerData.highestPoint)
                    {
                       
                        playerData.highestPoint = player.transform.position.y;
                    }

                    if (player.transform.position.y < playerData.highestPoint && !playerData.reachedApex && rb.linearVelocity.y <= 0f)
                    {
                        
                        playerData.reachedApex = true;
                        
                        
                        if (rb.linearVelocity.y < 0f)
                        {
                         
                            
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

      
       
        
        
        if (isTouchingHead)
        {
          
            player.StopUpwardVelocity();
        }
       
    }
}
