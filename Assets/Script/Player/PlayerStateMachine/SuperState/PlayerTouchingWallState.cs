using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool isTouchingWall;
    protected bool isGrounded;
    protected bool grabInput;
    protected int oldXinput;
    protected bool jumpInput;
    protected bool isTouchingLedge;
    public PlayerTouchingWallState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        grabInput = player.inputController.grabInput;
        jumpInput = player.inputController.runJumpInput;
        if (jumpInput)
        {
            Debug.LogWarning("Jump Input from touching wall");
            player.wallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.wallJumpState);
        }
        else if (isGrounded && !grabInput)
        {
            Debug.LogWarning("gounrded not grab");
            stateMachine.ChangeState(player.idleState);
        }
        else if (!isTouchingWall || (oldXinput != player.facingDirection && !grabInput))
        {
            Debug.LogWarning("not Touching wall but not facing the right direction");
            stateMachine.ChangeState(player.airState);
        }
        else if (isTouchingWall && !isTouchingLedge)
        {
            Debug.LogWarning("ledgeclimb");
            stateMachine.ChangeState(player.ledgeClimbState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingWall = player.IsWallDetected();
        isGrounded = player.IsGroundDetected();
        isTouchingLedge = player.CheckIfTouchingLedge();
        oldXinput = xInput;
        if (isTouchingWall && !isTouchingLedge)
        {
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

   
}
