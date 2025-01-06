using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        player.ZeroVelocity();
        playerData.isIdle = true;

    }

    public override void Exit()
    {
        base.Exit();
        playerData.isIdle = false;
    }

    public override void Update()
    {
        base.Update();
        
        if (xDirection == player.facingDirection && player.IsWallDetected())
        {
            //change state to wallslide state when slide animation is done
            return;
        }
        if (xDirection != 0 && !player.isBusy && !player.isAttacking)
        {
            
            stateMachine.ChangeState(player.moveState);
        }
        
    }
}
