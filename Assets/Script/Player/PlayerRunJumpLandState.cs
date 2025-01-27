using UnityEngine;

public class PlayerRunJumpLandState : PlayerGroundedState
{
    public PlayerRunJumpLandState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.inputController.isJumping = false;
        player.isFallingFromJump = false;
    }

    public override void Exit()
    {
        base.Exit();
       
    }

    public override void Update()
    {
        base.Update();
       
        if (!isExitingState)
        {
           if (xDirection != 0)
           {
              stateMachine.ChangeState(player.moveState);
           }
            else if(triggerCalled)
           {
              stateMachine.ChangeState(player.idleState);
           }  
            
        }
    }
}
