using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class PlayerSprintState : PlayerGroundedState
{
    public PlayerSprintState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        player.movementSpeed *= 2f;// 冲刺时的速度加倍
        Debug.Log("Player is sprinting");
    }

    public override void Exit()
    {
        base.Exit();
        player.movementSpeed /= 2; // 恢复原来的速度
    }

    public override void Update()
    {
        base.Update();
        moveDirection = Input.GetAxisRaw("Horizontal");
        
        if (!Keyboard.current.leftShiftKey.isPressed || moveDirection == 0)
        {
            stateMachine.ChangeState(player.moveState); // 释放左Shift键或停止移动时回到移动状态
        }
        // 检查是否按下左键进行膝击
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            stateMachine.ChangeState(player.kneeKickState);
        }
        player.SetVelocity(moveDirection * player.movementSpeed, rb.linearVelocity.y);
    }
        

    }
