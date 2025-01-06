using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class PlayerSprintState : PlayerGroundedState
{
    public PlayerSprintState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        playerData.movementSpeed *= 2f;// 冲刺时的速度加倍
        Debug.Log("Player is sprinting");
        playerData.isSprint = true;
    }

    public override void Exit()
    {
        base.Exit();
        playerData.movementSpeed /= 2; // 恢复原来的速度
        playerData.isSprint = false;
    }

    public override void Update()
    {
        base.Update();
        xDirection = Mathf.RoundToInt(player.inputController.norInputX);
        
        if (!Keyboard.current.leftShiftKey.isPressed || xDirection == 0)
        {
            stateMachine.ChangeState(player.moveState); // 释放左Shift键或停止移动时回到移动状态
        }
        // 检查是否按下左键进行膝击
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            stateMachine.ChangeState(player.kneeKickState);
        }
        player.SetVelocityX(xDirection * playerData.movementSpeed);
        // player.SetVelocity(xDirection * playerData.movementSpeed, rb.linearVelocity.y);
    }
        

    }
