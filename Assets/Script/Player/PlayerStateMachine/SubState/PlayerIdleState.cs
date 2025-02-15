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
        player.colliderManager.EnterCrouch(playerData.standColliderSize, playerData.standColliderOffset);
    }

    public override void Exit()
    {
        base.Exit();
        playerData.isIdle = false;
        player.colliderManager.ExitCrouch(playerData.standColliderSize, playerData.standColliderOffset);
    }

    public override void Update()
    {
        base.Update();
        
        

        if (!isExitingState)
        {
          
            if (xInput != 0 && !player.isBusy && !player.isAttacking)
            {
                stateMachine.ChangeState(player.moveState); 
            }else if (yInput == -1)
            {
                stateMachine.ChangeState(player.crouchIdleState);
            }
        }
        
    }
}