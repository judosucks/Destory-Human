using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        player.ZeroVelocity();
        
        Debug.LogWarning("idle isaiming"+player.isAiming);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.anim.SetBool("IsBusy",player.isBusy);
        Debug.Log("throwstate"+"isbusy"+player.isBusy);
        if (moveDirection == player.facingDirection && player.IsWallDetected())
        {
            //change state to wallslide state when slide animation is done
            return;
        }
        if (moveDirection != 0 && !player.isBusy && !player.isAttacking)
        {
            
            stateMachine.ChangeState(player.moveState);
        }
        
    }
}
