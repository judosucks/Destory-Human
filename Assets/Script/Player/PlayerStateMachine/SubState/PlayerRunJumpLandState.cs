using UnityEngine;

public class PlayerRunJumpLandState : PlayerGroundedState
{
    
    public PlayerRunJumpLandState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isFallingFromJump = false;
        playerData.isRunJumpLandState = true;
    }

    public override void Exit()
    {
        base.Exit();
       playerData.isRunJumpLandState = false;
    }

    public override void Update()
    {
        base.Update();
       
        if (!isExitingState)
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.moveState);
            }
            else if(triggerCalled)
            {
                Debug.Log("Trigger Called");
                stateMachine.ChangeState(player.idleState);
            }  
            
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}