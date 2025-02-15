using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerFallLandState : PlayerState
{
   private Vector2 holdPosition;
    public PlayerFallLandState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        holdPosition = player.transform.position;
        HoldPosition();
        player.isFallingFromEdge = false;
        player.isFallingFromJump = false;
        player.FallDownForceAndCountdown(1f);
        player.inputController.UseRunJumpInput();
     
    }

    
    
    public override void Update()
    {
        base.Update();
        // if (xInput != 0)
        // {
        //     stateMachine.ChangeState(player.moveState);
        // }else 
        HoldPosition();
        if (!isExitingState)
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.moveState);
            }
            else if(triggerCalled)
            {
                stateMachine.ChangeState(player.idleState);
            }  
            
        }

        
    }
    private void HoldPosition()
    {
        player.transform.position = holdPosition;
        player.SetVelocityX(0f);
        player.SetVelocityY(0f);
    }
    public override void Exit()
    {
        base.Exit();
       
    }

    public override void DoChecks()
    {
        base.DoChecks();
      
       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
