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
        if (GetYDirection() > 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * player.GetClimbUpForce());
        }

        if (GetYDirection() <= 0)
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
