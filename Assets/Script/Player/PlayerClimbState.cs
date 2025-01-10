using UnityEngine;

public class PlayerClimbState : PlayerState
{
    private bool isTouchingLedge;
    public PlayerClimbState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine,_playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        isTouchingLedge = player.CheckIfTouchingLedge();
        if (player.inputController.norInputY > 0)
        {
            Debug.Log("player is climbing");
            player.SetVelocityY(playerData.climbUpForce);
        }

        if (player.inputController.norInputY <= 0)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if (!isTouchingLedge)
        {
            Debug.Log("Touching Ledge");
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
            stateMachine.ChangeState(player.ledgeClimbState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
