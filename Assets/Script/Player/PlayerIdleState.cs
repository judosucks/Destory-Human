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
        
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        if (xDirection == playerData.facingDirection && player.IsWallDetected())
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
