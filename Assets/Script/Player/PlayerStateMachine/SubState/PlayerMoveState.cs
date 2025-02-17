using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class PlayerMoveState : PlayerGroundedState
{
    private bool isEdgeCheck;
    private bool isEdgeWallCheck;
    private bool edgeTouched;
    private int xInput;
    private bool sprintInput;
    private bool isGrounded;
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.IsGroundDetected();
        isEdgeCheck = player.CheckIfTouchingEdge();
        isEdgeWallCheck = player.CheckIfTouchingEdgeWall();
        if (!isEdgeCheck && isEdgeWallCheck && !edgeTouched && player.IsGroundDetected())
        {
            Debug.LogWarning("Edge Wall");
            edgeTouched = true;
            player.edgeClimbState.SetDetectedEdgePosition(player.transform.position);
        }
       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(!isEdgeCheck && isEdgeWallCheck && edgeTouched)
        {
            Debug.LogWarning("Edge Wall Check");
            stateMachine.ChangeState(player.edgeClimbState);
        }
        else if (isGrounded && player.isOnSlope && player.canWalkOnSlope)
        {
            Debug.Log($"State Change to SlopeClimbState | isGrounded: {isGrounded}, isOnSlope: {player.isOnSlope}, canWalkOnSlope: {player.canWalkOnSlope}");
            stateMachine.ChangeState(player.slopeClimbState);
        }
        
    }

    public override void Enter()
    {
        base.Enter();
        playerData.isRun = true;
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
        playerData.isRun = false;
        isEdgeCheck = false;
        isEdgeWallCheck = false;
        edgeTouched = false;
    }

    public override void Update()
    {
        base.Update();
        xInput = player.inputController.norInputX;
        sprintInput = player.inputController.sprintInput;
        player.CheckIfShouldFlip(xInput);

        if (!isExitingState)
        {
           // 基本的移动逻辑
           if (!sprintInput && xInput != 0) 
           {
               player.SetVelocityX(xInput * playerData.movementSpeed); // 常规移动速度
           }
           else if (xInput == 0) // 如果停止移动
           {
               stateMachine.ChangeState(player.idleState); // 进入待机状态
           }
           else if (sprintInput && xInput != 0) // 冲刺逻辑
           {
               stateMachine.ChangeState(player.sprintState); // 进入冲刺状态
           }
           else if (yInput == -1)
           {
               stateMachine.ChangeState(player.crouchMoveState);
           }
            
        }
    }
 
}


