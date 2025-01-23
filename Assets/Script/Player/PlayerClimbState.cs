using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerClimbState : PlayerState
{
    private bool isTouchingLedge;
    private bool touchedLedge;
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
        
        if (player.inputController.norInputY > 0)
        {
            Debug.Log("player is climbing");
            player.SetVelocityY(playerData.climbUpForce);
        }

        if (player.inputController.norInputY <= 0)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

       
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingLedge = player.CheckIfTouchingLedge();
        if (!isTouchingLedge)
        {
            touchedLedge = true;
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!isTouchingLedge && touchedLedge)
        {
            Debug.Log("Touching Ledge");
            touchedLedge = false;
            stateMachine.ChangeState(player.ledgeClimbState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        isTouchingLedge = false;
        touchedLedge = false;
    }
}
