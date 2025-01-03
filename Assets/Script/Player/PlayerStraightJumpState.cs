using UnityEngine;

public class PlayerStraightJumpState : PlayerState
{
    public PlayerStraightJumpState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = new Vector2(0, playerData.straightJumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (rb.linearVelocity.y < 0 && xDirection == 0)
        {
            stateMachine.ChangeState(player.straightJumpAirState);
        }

        if (rb.linearVelocity.y < 0 && xDirection != 0)
        {
            stateMachine.ChangeState(player.airState);
        }
       
    }
}
