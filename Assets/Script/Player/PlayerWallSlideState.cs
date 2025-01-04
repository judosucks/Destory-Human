using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
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

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }
        
        if (xDirection != 0 && player.facingDirection != xDirection)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (yDirection > 0)
        {
            stateMachine.ChangeState(player.climbState);
        }
        if (yDirection < 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * -playerData.wallSlideDownForce);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * .7f);
        }
            
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }



}
}
