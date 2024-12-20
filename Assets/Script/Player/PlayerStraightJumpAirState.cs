using UnityEngine;

public class PlayerStraightJumpAirState : PlayerState
{
    private bool isTouchingLedge;
    public PlayerStraightJumpAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
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
        
        isTouchingLedge = player.CheckIfTouchingLedge();
        if (player.IsWallDetected() && !isTouchingLedge && stateMachine.currentState != player.airState)
        {
            Debug.Log("this is straight jump air state is touching wall but not ledge");
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
            stateMachine.ChangeState(player.ledgeClimbState);
        }
        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        if (player.IsGroundDetected())
        {
            Debug.Log("this is straight jump air state is touching ground");
            stateMachine.ChangeState(player.idleState);
        }

        
        
    }
}
