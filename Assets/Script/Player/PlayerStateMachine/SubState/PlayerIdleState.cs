using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerIdleState : PlayerGroundedState
{
    private bool isGrounded;
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
        Debug.Log("PlayerMoveState Enter Called");
        player.SlopeCheck(); // 刷新坡地检测
        if (player.isOnSlope && player.canWalkOnSlope)
        {
            Debug.Log("Switching to SlopeClimbState on Enter");
            stateMachine.ChangeState(player.slopeClimbState);
        }

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

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (isGrounded && player.isOnSlope && player.canWalkOnSlope)
        {
            Debug.Log($"State Change to SlopeClimbState | isGrounded: {isGrounded}, isOnSlope: {player.isOnSlope}, canWalkOnSlope: {player.canWalkOnSlope}");
            stateMachine.ChangeState(player.slopeClimbState);
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.IsGroundDetected();
    }
    
}