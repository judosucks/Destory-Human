using UnityEngine;

public class PlayerFallLandState : PlayerState
{
    private int xInput;
    private bool isGrounded;
    public PlayerFallLandState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isFallingFromEdge = false;
    }

    public override void Update()
    {
        base.Update();
        if (xInput != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
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
        isGrounded = player.IsGroundDetected();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
