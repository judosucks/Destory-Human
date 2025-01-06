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
        bool isTouchingGround = player.IsGroundDetected();
        bool isTouchingWall = player.IsWallDetected();
        bool isTouchingLedge = player.CheckIfTouchingLedge();
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }
        
       
        if (Mathf.RoundToInt(yDirection) > 0)
        {
            stateMachine.ChangeState(player.climbState);
        }
        if (Mathf.RoundToInt(yDirection) < 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * -playerData.wallSlideDownForce);
        }
        else 
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * .7f);
        }
            
        if (isTouchingGround)
        {
            stateMachine.ChangeState(player.idleState);
        }else if (!isTouchingWall || Mathf.RoundToInt(xDirection) != player.facingDirection)
        {
            stateMachine.ChangeState(player.airState);
        }



}
}
