using UnityEngine;

public class PlayerAirState : PlayerState
{
    private bool isTouchingLedge;
    private bool isTouchingWall;
    private bool isTouchingGround;
    private bool isWallBackDetected;
    private int xInput;
    private bool isTouchingHead;
    private bool isTouchingRightGround;
    private bool isTouchingLeftGround;
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("current velocity:"+player.CurrentVelocity.y);
        playerData.isInAir = true;

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingLedge = player.CheckIfTouchingLedge();
        isTouchingWall = player.IsWallDetected();
        isTouchingGround = player.IsGroundDetected();
        isWallBackDetected = player.isWallBackDetected();
        isTouchingHead = player.CheckIfTouchingHead();
        isTouchingRightGround = player.isRightGroundDetected();
        isTouchingLeftGround = player.isLeftGroundDetected();
        xInput = player.inputController.norInputX;
        if (isTouchingWall && !isTouchingLedge)
        {
            Debug.Log("dochecks setdetectedposition");
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    public override void Exit()
    {
        base.Exit();
       playerData.isInAir = false;
       isTouchingGround = false;
       isTouchingWall = false;
       isTouchingLedge = false;
       isWallBackDetected = false;
       isTouchingHead = false;
       
    }

    
    public override void Update()
    {
        base.Update();




        if (rb.linearVelocity.y <= 0 && isTouchingRightGround&& !isTouchingLeftGround || rb.linearVelocity.y <= 0 &&  isTouchingLeftGround&& !isTouchingRightGround)
        {
            Debug.LogWarning("PlayerJumpAirState: player is not on the ground, but is moving upwards");
            player.SetVelocity(1f * player.facingDirection,1);
        }

        if (xInput != 0)
        {
            player.SetVelocity(playerData.movementSpeed * .6f * xInput, player.CurrentVelocity.y);
        }
         if (isTouchingHead)
         {
             Debug.Log("touching head");
             player.StopUpwardVelocity();
         }
        // 如果触碰到墙但不是ledge，并且玩家还在下降态势时
        // Refine ledge detection; ensure ledge transitions are prioritized
        if (isTouchingWall && !isTouchingLedge &&!isTouchingGround)
        {
            Debug.Log("Detected ledge, transitioning to LedgeClimbState.");
            stateMachine.ChangeState(player.ledgeClimbState);
        }

// Ensure wall slide only happens when actively sliding down a wall
        else if (isTouchingWall && player.CurrentVelocity.y <= 0 && xInput == player.facingDirection)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        
        if (!isTouchingGround)
        {   
            rb.linearVelocity += Vector2.down * (playerData.gravityMultiplier* Time.deltaTime);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -playerData.maxFallSpeed, Mathf.Infinity));
            player.CheckForCurrentVelocity();
        }
        // else if (!isTouchingGround && xInput != 0)
        // {
        //     player.SetVelocityX(xInput *playerData.movementSpeed * .8f);
        // }

        
        
        if (isTouchingGround && player.CurrentVelocity.y <0.01f)
        {
            Debug.Log("land");
            stateMachine.ChangeState(player.runJumpLandState);
        }

        if (isTouchingGround && Mathf.Abs(player.CurrentVelocity.x) < 0.1f)
        {
            Debug.Log("touching ground and x velocity is 0");
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            player.CheckForCurrentVelocity();
        }
        
        
        
    }
}