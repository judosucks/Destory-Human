using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter move");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        moveDirection = Input.GetAxisRaw("Horizontal");
        SprintInput();//衝刺輸入處理
        if (!player.isAttacking)
        {
            
        player.SetVelocity(moveDirection * player.movementSpeed,rb.linearVelocity.y);
            
        }
        
        if (moveDirection == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void SprintInput()
    {
        if (Keyboard.current.leftShiftKey.isPressed && moveDirection != 0)
        {
            stateMachine.ChangeState(player.sprintState); // 切换到冲刺状态
        }
    }

}
