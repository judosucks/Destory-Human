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
    private bool isTouchingRightGround;
    private bool isTouchingLeftGround;
    public PlayerStraightJumpAirState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
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
        isTouchingRightGround = player.isRightGroundDetected();
        isTouchingLeftGround = player.isLeftGroundDetected();
        xInput = player.inputController.norInputX;
        if (isTouchingWall && !isTouchingLedge)
        {
            Debug.Log("setdetectedposition straightjumpairstate");
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("current velocity sj"+player.CurrentVelocity.y);
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
        
    }
    
    public override void Update()
    {
        base.Update();
        if (xInput != 0)
        {
            player.SetVelocity(playerData.movementSpeed * .6f * xInput, player.CurrentVelocity.y);
        }

        if (rb.linearVelocity.y <= 0 && isTouchingRightGround&& !isTouchingLeftGround || rb.linearVelocity.y <= 0 &&  isTouchingLeftGround&& !isTouchingRightGround)
        {
            Debug.LogWarning("PlayerStraightJumpAirState: player is not on the ground, but is moving upwards");
            player.SetVelocity(1f * player.facingDirection,1);
        }
        // 墙相关的状态切换逻辑
        if (isTouchingWall && !isTouchingLedge && !isTouchingGround)
        {
            Debug.Log("PlayerStraightJumpAirState: touching wall but not ledge, transitioning to LedgeClimbState");
            stateMachine.ChangeState(player.ledgeClimbState);
        }

        if (isTouchingWall && player.CurrentVelocity.y <= 0 && xInput == player.facingDirection)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
       
        if (!isTouchingGround)
        {   
            rb.linearVelocity += Vector2.down * (playerData.gravityMultiplier* Time.deltaTime);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -playerData.maxFallSpeed, Mathf.Infinity));
            player.CheckForCurrentVelocity();
        }
        // else if (!isTouchingGround && xInput != 0)
        // {
        //     player.SetVelocityX(xInput * playerData.movementSpeed * .8f);
        // }

        if (isTouchingGround && player.CurrentVelocity.y <0.01f )
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.runJumpLandState);
                return;
            }
            
            stateMachine.ChangeState(player.straightJumpLandState);
        }

        
        if (isTouchingGround && Mathf.Abs(player.CurrentVelocity.x) < 0.1f)
        {
            Debug.Log("touching ground and x velocity is 0");
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            player.CheckForCurrentVelocity();
        }
        if (isTouchingHead)
        {
            Debug.Log("touching head");
            player.StopUpwardVelocity();
        }
       
    }
}
