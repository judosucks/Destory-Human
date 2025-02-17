using UnityEngine;

public class PlayerSlopesClimbState : PlayerGroundedState
{
    
    public PlayerSlopesClimbState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        
    }
    public override void Update()
    {
        base.Update();
        if (player.isOnSlope && xInput !=0)
        { 
            Debug.LogWarning("Slope");
            player.SetVelocityX(playerData.movementSpeed * player.slopeNormalPerp.x * -xInput);
            player.SetVelocityY(playerData.movementSpeed * player.slopeNormalPerp.y * -xInput);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void DoChecks()
    {
        base.DoChecks();
        // SlopeCheck();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    

    
}
