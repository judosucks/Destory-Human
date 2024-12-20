using UnityEngine;

public class PlayerAirState : PlayerState
{
    private bool isTouchingLedge;
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Exit()
    {
        base.Exit();
       
    }

    public override void Update()
    {
        base.Update();
        
        // 根据输入调整玩家的水平速度
        var velocity = rb.linearVelocity;
        velocity.x = moveDirection * player.horizontalSpeed;
        rb.linearVelocity = velocity;
        isTouchingLedge = player.CheckIfTouchingLedge();
        if (player.IsWallDetected() && !isTouchingLedge && stateMachine.currentState != player.straightJumpAirState)
        {
            Debug.Log("run jump air is touching wall but not ledge");
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
            stateMachine.ChangeState(player.ledgeClimbState);
        }

        
        if (player.IsWallDetected()) stateMachine.ChangeState(player.wallSlideState);
        if (player.IsGroundDetected()) stateMachine.ChangeState(player.idleState);
    }
}