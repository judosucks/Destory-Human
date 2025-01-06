using Unity.VisualScripting;
using UnityEngine;

public class PlayerStraightJumpAirState : PlayerState
{
    private bool isTouchingLedge;
    private bool isTouchingWall;
    private bool isTouchingGround;
    private bool isWallBackDetected;
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
        if (isTouchingWall && !isTouchingLedge)
        {
            Debug.Log("setdetectedposition straightjumpairstate");
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
        
    }
    
    public override void Update()
    {
        base.Update();
        
        
        if (xDirection != 0)
        {
            player.MoveInAir();
        }
        // 墙相关的状态切换逻辑
        if (isTouchingWall && !isTouchingLedge)
        {
            Debug.Log("PlayerStraightJumpAirState: touching wall but not ledge, transitioning to LedgeClimbState");
            stateMachine.ChangeState(player.ledgeClimbState);
        }

        if (isTouchingWall && player.CurrentVelocity.y <= 0 && Mathf.RoundToInt(xDirection) == player.facingDirection)
        {
            Debug.Log("xDirection: " + Mathf.RoundToInt(xDirection) + " facingDirection: " + player.facingDirection +
                      " PlayerStraightJumpAirState: transitioning to WallSlideState");
            stateMachine.ChangeState(player.wallSlideState);
        }
       
        if (!isTouchingGround)
            
        {Debug.Log("current velocity y: " + player.CurrentVelocity.y + "rb"+rb.linearVelocity.y);
            rb.linearVelocity += Vector2.down * (playerData.gravityMultiplier* Time.deltaTime);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -playerData.maxFallSpeed, Mathf.Infinity));
            
        }if (isTouchingGround && player.CurrentVelocity.y <0.01f )
        {
            if (xDirection != 0)
            {
                stateMachine.ChangeState(player.runJumpLandState);
                return;
            }
            Debug.Log("land");
            stateMachine.ChangeState(player.straightJumpLandState);
        }

        if (isWallBackDetected)
        {
            Debug.Log("wall back detected player wall slide state");
            stateMachine.ChangeState(player.wallSlideState);
        }
        
    }
}
