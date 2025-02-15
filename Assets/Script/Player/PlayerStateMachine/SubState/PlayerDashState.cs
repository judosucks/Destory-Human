using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class PlayerDashState : PlayerState
{
    public bool IsDashing { get; private set; }

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
    }

    

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter dash");
        player.skill.dashSkill.CloneOnDash();
        IsDashing = true;
        startTime = playerData.dashDuration;
       
    }

    public override void Exit()
    {
        base.Exit();
        player.skill.dashSkill.CloneOnArrival();
        IsDashing = false;
        player.SetVelocityX(0);
    }

    public override void Update()
    {
        base.Update();
        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        // 设置冲刺速度
        player.SetVelocityX(playerData.dashSpeed * player.facingDirection);

        // if (Mouse.current.leftButton.wasPressedThisFrame || (gamepad != null && gamepad.buttonWest.wasPressedThisFrame))
        // {
        //     stateMachine.ChangeState(player.primaryAttackState);
        //     return;
        // }
        
        
        // if (canPerformDashAttack)
        // {
        //     
        //     if (mouse.leftButton.wasPressedThisFrame || (gamepad != null && gamepad.buttonWest.wasPressedThisFrame))
        //     {
        //         
        //         stateMachine.ChangeState(player.crossKickState);
        //     }
        //     
        //     
        // }
        // else if(startTime < 0f)
        // {
        //     
        //     stateMachine.ChangeState(player.idleState);
        // }
    }

   

    
}