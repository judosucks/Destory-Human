using UnityEngine;

public class PlayerStraightJumpAirState : PlayerState
{
    private bool isTouchingLedge;
    public PlayerStraightJumpAirState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        isTouchingLedge = player.CheckIfTouchingLedge();
        Debug.Log("enter air state");
        if (isTouchingLedge || player.IsWallDetected())
        {
            Debug.Log("enter wallslide state");
            stateMachine.ChangeState(player.wallSlideState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        
    }

    // public override void Update()
    // {
    //     base.Update();
    //     //根据输入调整玩家的水平速度
    //     var velocity = rb.linearVelocity;
    //     velocity.x = xDirection * player.horizontalSpeed;
    //     rb.linearVelocity = velocity;
    //     isTouchingLedge = player.CheckIfTouchingLedge();
    //     if (player.IsWallDetected() && !isTouchingLedge)
    //     {
    //         Debug.Log("this is straight jump air state is touching wall but not ledge");
    //         player.ledgeClimbState.SetDetectedPosition(player.transform.position);
    //         stateMachine.ChangeState(player.ledgeClimbState);
    //     }
    //     if (player.IsWallDetected())
    //     {
    //         stateMachine.ChangeState(player.wallSlideState);
    //     }
    //     if (player.IsGroundDetected())
    //     {
    //         Debug.Log("this is straight jump air state is touching ground");
    //         stateMachine.ChangeState(player.idleState);
    //     }
    //
    //     
    //     
    // }
    public override void Update()
    {
        base.Update();

        // 玩家水平移动
        var velocity = rb.linearVelocity;
        velocity.x = xDirection * playerData.horizontalSpeed;
        rb.linearVelocity = velocity;

        // 跟踪墙和ledge状态
        isTouchingLedge = player.CheckIfTouchingLedge();
        bool isTouchingWall = player.IsWallDetected();
        bool isTouchingGround = player.IsGroundDetected();

        // 墙相关的状态切换逻辑
        if (isTouchingWall && !isTouchingLedge && rb.linearVelocity.y < 0)
        {
            Debug.Log("PlayerStraightJumpAirState: touching wall but not ledge, transitioning to LedgeClimbState");
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
            stateMachine.ChangeState(player.ledgeClimbState);
        }

        if (isTouchingWall && rb.linearVelocity.y < 0 && Mathf.Abs(xDirection) > 0.1f)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        else if (isTouchingGround)
        {
            Debug.Log("PlayerStraightJumpAirState: touching ground");
            stateMachine.ChangeState(player.idleState);
        }
    }
}
