using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class PlayerSprintState : PlayerGroundedState
{
    private int xInput;
    private bool sprintInput;
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
        xInput = player.inputController.norInputX;
        sprintInput = player.inputController.sprintInput;
        if (!isExitingState)
        {
           if (sprintInput && xInput == 0)
           {
             player.inputController.UseSprintInput();
             stateMachine.ChangeState(player.idleState);   
           }
           else if (!sprintInput && xInput != 0)
           {
               stateMachine.ChangeState(player.moveState);
           }
           else if (!sprintInput && xInput == 0)
           {
               stateMachine.ChangeState(player.idleState);
           }
           // 检查是否按下左键进行膝击
           else if (Mouse.current.leftButton.wasPressedThisFrame)
           {
               stateMachine.ChangeState(player.kneeKickState);
           }
           player.SetVelocityX(xInput * playerData.movementSpeed);
            
        }
        // player.SetVelocity(xInput * playerData.movementSpeed, rb.linearVelocity.y);
    }
        

    }
