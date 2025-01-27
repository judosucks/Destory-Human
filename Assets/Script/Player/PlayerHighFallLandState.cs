using UnityEngine;

public class PlayerHighFallLandState : PlayerState
{
    private int xInput;
    public PlayerHighFallLandState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.inputController.isJumping = false;
        player.isFallingFromEdge = false;
        player.isFallingFromJump = false;
        CameraManager.instance.newCamera.ShakeCamera(.9f, .9f);
        player.FallDownForceAndCountdown(1.5f);
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

    public override void Exit()
    {
        base.Exit();
    }

    public override void DoChecks()
    {
        base.DoChecks();
        xInput = player.inputController.norInputX;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
