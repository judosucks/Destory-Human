using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerFallLandState : PlayerGroundedState
{
    public PlayerFallLandState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
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
