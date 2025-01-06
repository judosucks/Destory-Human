using UnityEngine;

public class PlayerAirState : PlayerState
{
    private bool isTouchingLedge;
    private bool isTouchingWall;
    private bool isTouchingGround;
    private bool isWallBackDetected;

    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
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
    }

    
    public override void Update()
    {
        base.Update();
    
       
        
       
         
         if (xDirection != 0)
         {
           player.MoveInAir();
         }
        // 如果触碰到墙但不是ledge，并且玩家还在下降态势时
        // Refine ledge detection; ensure ledge transitions are prioritized
        if (isTouchingWall && !isTouchingLedge)
        {
            Debug.Log("Detected ledge, transitioning to LedgeClimbState.");
            stateMachine.ChangeState(player.ledgeClimbState);
        }

// Ensure wall slide only happens when actively sliding down a wall
        else if (isTouchingWall && player.CurrentVelocity.y <= 0 && Mathf.RoundToInt(xDirection) == player.facingDirection)
        {
            Debug.Log("xDirection: " + Mathf.RoundToInt(xDirection) + " facingDirection: " + player.facingDirection +
                      " PlayerStraightJumpAirState: transitioning to WallSlideState");
            stateMachine.ChangeState(player.wallSlideState);
        }
        
        if (!isTouchingGround)
        {   
            
            rb.linearVelocity += Vector2.down * (playerData.gravityMultiplier* Time.deltaTime);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -playerData.maxFallSpeed, Mathf.Infinity));
            
            
        }

        if (isWallBackDetected)
        {
            Debug.Log("wall back detected player wall slide state");
            stateMachine.ChangeState(player.wallSlideState);
        }
        
        if (isTouchingGround && player.CurrentVelocity.y <0.01f)
        {
            Debug.Log("land");
            stateMachine.ChangeState(player.runJumpLandState);
        }

        
        
        
    }
}