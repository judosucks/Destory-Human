using UnityEngine;

public class PlayerStraightJumpState : PlayerState
{
    public PlayerStraightJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = new Vector2(0, player.straightJumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (rb.linearVelocity.y < 0 && moveDirection == 0)
        {
            stateMachine.ChangeState(player.straightJumpAirState);
        }
       
    }
}
