using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerClimbState : PlayerState
{
    private bool isTouchingLedge;
    private bool touchedLedge;
    private bool isTouchingLedge2;
    private bool isTouchingLedge3;
    private LedgeTriggerDetection ledgeTriggerDetection;
    public PlayerClimbState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine,_playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ledgeTriggerDetection = player.GetComponentInChildren<LedgeTriggerDetection>();
    }

    public override void Update()
    {
        base.Update();
        
        if (player.inputController.norInputY > 0)
        {
            Debug.Log("player is climbing"+rb.linearVelocity.y);
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
        isTouchingLedge2 = player.CheckIfTouchingLedgeTwo();
        isTouchingLedge3 = ledgeTriggerDetection.isTouchingLedge;
        if (!isTouchingLedge3 && rb.linearVelocity.x < 0.1f && !(rb.linearVelocity.x >0.2f)|| !isTouchingLedge3 && !(rb.linearVelocity.x > 0.1f))
        {
            touchedLedge = true;
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
        }

        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        if (!isTouchingLedge3 && rb.linearVelocity.x < 0.1f && !(rb.linearVelocity.x >0.2f)|| !isTouchingLedge3 && !(rb.linearVelocity.x > 0.1f))
        {
           
            touchedLedge = false;
            stateMachine.ChangeState(player.ledgeClimbState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        isTouchingLedge = false;
        touchedLedge = false;
        isTouchingLedge2 = false;
        isTouchingLedge3 = false;
    }
}
