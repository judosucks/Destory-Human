using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Collections.Generic;
using System.Collections;
public class PlayerWallJumpState : PlayerAbilityState
{
    private int wallJumpDirection;
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Wall Jump");
        player.inputController.UseRunJumpInput();
        player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle,wallJumpDirection);
        player.CheckIfShouldFlip(wallJumpDirection);
        player.jumpState.DecrementAmountOfJumpsLeft();
    }

    public override void Exit()
    {
        base.Exit();
        
        
    }

    public override void Update()
    {
        base.Update();
        player.anim.SetFloat("yVelocity",rb.linearVelocity.y);
        player.anim.SetFloat("xVelocity",Mathf.Abs(rb.linearVelocity.x));
        if (Time.time >= startTime + playerData.wallJumpTime)
        {
            isAbilityDone = true;
        }
        
    }
    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -player.facingDirection;
        }
        else
        {
            wallJumpDirection = player.facingDirection;
        }
    }
}
