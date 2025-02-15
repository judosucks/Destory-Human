using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    private bool isTouchingGround;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    private int xInput;
    private int yInput;
    private bool isClimbing;
    private bool isWallBottomDetected;
    private bool runJumpInput;
    private bool sprintJumpInput;
    private bool straightJumpInput;
    private bool isEdgeGrounded;
    private bool isAnimationReversed;
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData,
        string _animBoolName) : base(_player,
        _stateMachine, _playerData, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        playerData.isWallSliding = true;
        playerData.isWallSlidingState = true;
        player.inputController.UseRunJumpInput();
        player.SetVelocityY(-playerData.wallSlideVelocity);
        Debug.Log("entering wall slide state"+player.inputController.runJumpInput+""+player.inputController.sprintJumpInput);
    }

    public override void Exit()
    {
        base.Exit();
        playerData.isWallSliding = false;
        playerData.isWallSlidingState = false;
        isTouchingGround = false;
        isTouchingWall = false;
        isTouchingLedge = false;
        isClimbing = false;
        isWallBottomDetected = false;
        runJumpInput = false;
        sprintJumpInput = false;
        straightJumpInput = false;
        playerData.reachedApex = false;
        isEdgeGrounded = false;
        isAnimationReversed = false;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingGround = player.IsGroundDetected();
        isTouchingWall = player.IsWallDetected();
        isTouchingLedge = player.CheckIfTouchingLedge();
        isWallBottomDetected = player.IsWallBottomDetected();
        isEdgeGrounded = player.IsEdgeGroundDetected();
       
    }
    

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
       
    }

    public override void Update()
    {
        base.Update();
        xInput = player.inputController.norInputX;
        yInput = player.inputController.norInputY;
        runJumpInput = player.inputController.runJumpInput;
        sprintJumpInput = player.inputController.sprintJumpInput;
        if (!isExitingState)
        {
           player.SetVelocityY(-playerData.wallSlideVelocity);
           if (yInput == 0 && grabInput)
           {
              stateMachine.ChangeState(player.wallGrabState);
           }
        }
    }
}


