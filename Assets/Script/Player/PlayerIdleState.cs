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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (moveDirection == player.facingDirection && player.IsWallDetected())
        {
            Debug.Log("from idle return");
            return;
        }
        if (moveDirection != 0 && !player.GetIsBusy() && !player.isAttacking)
        {
            Debug.Log("player isattacking" + player.isAttacking);
            stateMachine.ChangeState(player.moveState);
        }
    }
}
