using Unity.VisualScripting;
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
    public bool oldIsTouchingGround;
    private bool isFirstLand;
    private float fallTime = 0f; 
    private bool isTouchingLedgeTwo;
    

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

        player.ApplyGravityAndClampVelocity();


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();


        if (playerData.reachedApex && !isTouchingGround && player.IsRightGroundDetected() &&
            !player.IsLeftGroundDetected() && !isTouchingWall || playerData.reachedApex &&
            player.IsLeftGroundDetected() && !player.IsRightGroundDetected() && !isTouchingWall && !isTouchingGround)
        {
            if (Mathf.RoundToInt(xDirection) != player.facingDirection && Mathf.RoundToInt(xDirection) != 0)
            {
                playerData.reachedApex = false;
                return;
            }

            Debug.LogWarning("entering reachedapex");
            if (!player.IsFrontBottomDetected())
            {
                Debug.LogWarning("stand IsFrontBottomDetected()");
                playerData.reachedApex = false;
                player.MoveTowardSmooth(playerData.moveDirection * player.facingDirection, playerData.moveDistance);
                stateMachine.ChangeState(player.standState);
                

            }


        }

        if (player.CurrentVelocity.y < 0)
        {
            if (fallTime > 0.2f && player.isFallingFromJump || fallTime > 0.2f && player.isFallingFromEdge)
            {
                playerData.reachedApex = false;    
                Debug.Log("playerData.reachedApex false");
            }
            
            
        }

        // 如果触碰到墙但不是ledge，并且玩家还在下降态势时
        // Refine ledge detection; ensure ledge transitions are prioritized
        if (isTouchingWall && !isTouchingLedge && !isTouchingGroundBottom&&isDetecting)
        {

            if (rb.linearVelocity.x > -0.1f && player.facingDirection == -1&& rb.linearVelocity.y < 0 ||
                rb.linearVelocity.x < 0.1f && player.facingDirection == 1 && rb.linearVelocity.y < 0) ;
            {
                if (fallTime < 0.1f)
                {
                    Debug.LogWarning("falltime"+fallTime+"rb"+rb.linearVelocity.x+"rb"+rb.linearVelocity.y);
                    stateMachine.ChangeState(player.ledgeClimbState);
                }
            }


        }

        if (isWallBackDetected && player.isFallingFromEdge || isWallBackDetected && player.isFallingFromJump)
        {
         
            player.isFallingFromEdge = false;
            player.MoveTowardSmooth(playerData.moveDirection * player.facingDirection, playerData.moveDistance);
        }

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
        xInput = player.inputController.norInputX;
        isTouchingLeftEdge = player.isNearLeftEdge;
        isTouchingRightEdge = player.isNearRightEdge;
        isTouchingGroundBottom = player.IsBottomGroundDetected();
        isTouchingWallBottom = player.IsWallBottomDetected();
        isWallTopDetected = player.IsWallTopDetected();
        isEdgeGrounded = player.IsEdgeGroundDetected();
        isTouchingLedgeTwo = player.CheckIfTouchingLedgeTwo();
        isTouchingWallBottom = player.IsWallBottomDetected();
        if (isTouchingWall && !isTouchingLedge && !isTouchingGroundBottom)
        {
            
            if (rb.linearVelocity.x > -0.1f && player.facingDirection == -1&& rb.linearVelocity.y < 0 ||
                rb.linearVelocity.x < 0.1f && player.facingDirection == 1 && rb.linearVelocity.y < 0)
            {
                if (fallTime < 0.1f)
                {
                Debug.LogWarning("falltime"+fallTime+"rb"+rb.linearVelocity.x+"rb"+rb.linearVelocity.y);
                isDetecting = true;
                player.ledgeClimbState.SetDetectedPosition(player.transform.position);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        playerData.isInAir = false;
        player.inputController.isJumping = false;
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
        isTouchingGround = false;
        isFirstLand = false;
        oldIsTouchingGround = false;
        isTouchingWallBottom = false;
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
                stateMachine.ChangeState(player.highFallLandState);
                return;
            }else
            {
                
                isFirstLand = true;
            }

            // 其次处理中等高度掉落
            
            if (isFirstLand&& fallDistance >= player.midFallThreshold)
            {

                
                stateMachine.ChangeState(player.fallLandState);
                return;


            }
            else
            {
                isFirstLand = false;
            }

            stateMachine.ChangeState(player.runJumpLandState);
            isFirstLand = false;

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

        if (isTouchingHead)
        {
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

                if (player.transform.position.y < playerData.highestPoint && !playerData.reachedApex &&
                    rb.linearVelocity.y <= 0f)
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

        
    }
    
}
// else if (!isTouchingGround && xInput != 0)
        // {
        //     player.SetVelocityX(xInput *playerData.movementSpeed * .8f);
        // }

      
        
      

        

        
        
        
        
    

   
