using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Wall Jump");
        player.inputController.CancelAllJumpInput();
        stateTimer = .4f;
        player.SetVelocityY(playerData.jumpForce);
        player.SetVelocityX(playerData.verticalAirSpeed * -player.facingDirection);
        player.CheckForCurrentVelocity();
        player.Flip();
    }

    public override void Exit()
    {
        base.Exit();
        
        
    }

    public override void Update()
    {
        base.Update();
        // Check if state should transition
        if (stateTimer < 0) 
        {
            Debug.Log("Wall Jump");
            // When air state is entered, continue moving in -facingDirection
            // player.SetVelocityX(5 * -player.facingDirection);
            stateMachine.ChangeState(player.airState); 
        }

        if (player.IsGroundDetected()) 
        {
            Debug.LogWarning("land");
            // When grounded, apply horizontal velocity and switch to idle state
            player.SetVelocityX(0); // Stop horizontal movement on landing
            stateMachine.ChangeState(player.idleState);
        }
        

        

    }
}
