using UnityEngine;

public class PlayerStraightJumpLandState :PlayerGroundedState
{
    private int xInput;
    public PlayerStraightJumpLandState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.isFallingFromJump = false;
    }

    public override void Exit()
    {
        base.Exit();
       
    }

    public override void DoChecks()
    {
        base.DoChecks();
        xInput = player.inputController.norInputX;
    }

    public override void Update()
    {
        base.Update();
        
        if (xInput != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
        if(triggerCalled)
        {
            
            stateMachine.ChangeState(player.idleState);
        }
    }
}
