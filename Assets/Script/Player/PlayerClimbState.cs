using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerClimbState : PlayerState
{
    private bool isTouchingLedge;
    private bool touchedLedge;
    private bool isTouchingLedge2;
    private bool isTouchingLedge3;

 
    private bool isTouchingWall;
    private LedgeTriggerDetection ledgeTriggerDetection;
    private bool isClimbing;
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
        if (yDirection> 0 && isTouchingWall && !touchedLedge)
        {
            
            isClimbing = true;
            player.SetVelocityY(playerData.climbUpForce);
        }

        if (yDirection <= 0 && isTouchingWall && !touchedLedge)
        {
            isClimbing = false;
            stateMachine.ChangeState(player.wallSlideState);
        }

    }
  
    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingLedge = player.CheckIfTouchingLedge();
        isTouchingLedge2 = player.CheckIfTouchingLedgeTwo();
        isTouchingLedge3 = ledgeTriggerDetection.isTouchingLedge;
        isTouchingWall = player.IsWallDetected();
        if (!isTouchingLedge && isTouchingWall && isClimbing)
        {
            isClimbing = false;
           touchedLedge = true;
           player.ledgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        if (!isTouchingLedge && isTouchingWall && touchedLedge)
        {
            
            isClimbing = false;
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
        isTouchingWall = false;
        isClimbing = false;
        isTouchingLedge = false;
    }
}
