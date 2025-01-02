using UnityEngine;

public class PlayerAirState : PlayerState
{
    private bool isTouchingLedge;
    private float velocityY;
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        velocityY = rb.linearVelocity.y;
        Debug.Log(velocityY+"velocityY");
        isTouchingLedge = player.CheckIfTouchingLedge();
        Debug.Log("enter air state");
        if (isTouchingLedge || player.IsWallDetected())
        {
            Debug.Log("enter wallslide state");
            stateMachine.ChangeState(player.wallSlideState);
        }

        if (player.IsWallDetected() && player.facingDirection != GetXDirection())
        {
            Debug.Log("change to wall slide");
          stateMachine.ChangeState(player.wallSlideState);   
        }

        // Adjust the raycast to only detect back walls when the player is at specific heights
        if (player.isWallBackDetected() && player.rb.linearVelocity.y < 0)
        {
            Debug.Log("Player back is touching wall in air");
            if (!player.IsGroundDetected())
            {
                stateMachine.ChangeState(player.wallSlideState);
            }
        }
        else
        {
            Debug.Log("False back wall detection avoided.");
        }
        // Confirm animation clip info and ground state
        AnimatorClipInfo[] animatorClipInfo = player.anim.GetCurrentAnimatorClipInfo(0);
        foreach (AnimatorClipInfo clipInfo in animatorClipInfo)
        {
            Debug.Log("Current Clip: " + clipInfo.clip.name);

            // Check if still mid-air during landing animations
            if (clipInfo.clip.name.Contains("_run"))
            {
                Debug.Log("Animation suggests run, but player still airborne.");
                // Prevent wall slide or unintended transitions due to animations
                if (!player.IsGroundDetected() && Mathf.Approximately(rb.linearVelocity.y, 0) && rb.gravityScale > 0)
                {
                    Debug.Log("Player stuck mid-air, forcing fall.");
                    rb.linearVelocity = Vector2.down * 1f; // Apply a small downward velocity
                }
                if (!player.IsGroundDetected() && !player.IsWallDetected()) continue;
            }

            // Handle air-to-ground transitions properly
            if (clipInfo.clip.name.Contains("_run") && player.IsGroundDetected())
            {
                Debug.Log("Ground detected during run animation.");
                stateMachine.ChangeState(player.idleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
       
    }

    // public override void Update()
    // {
    //     base.Update();
    //     
    //     //根据输入调整玩家的水平速度
    //     var velocity = rb.linearVelocity;
    //     velocity.x = xDirection * player.horizontalSpeed;
    //     rb.linearVelocity = velocity;
    //     isTouchingLedge = player.CheckIfTouchingLedge();
    //     bool isTouchingWall = player.IsWallDetected();
    //     bool isTouchingGround = player.IsGroundDetected();
    //     if (player.IsWallDetected() && !isTouchingLedge)
    //     {
    //         Debug.Log("run jump air is touching wall but not ledge");
    //         player.ledgeClimbState.SetDetectedPosition(player.transform.position);
    //         stateMachine.ChangeState(player.ledgeClimbState);
    //     }
    //
    //     
    //     if (player.IsWallDetected()) stateMachine.ChangeState(player.wallSlideState);
    //     if (player.IsGroundDetected()) stateMachine.ChangeState(player.idleState);
    //     
    // }
    public override void Update()
    {
        base.Update();
    
        // 玩家水平移动
        var velocity = rb.linearVelocity;
        velocity.x = xDirection * player.horizontalSpeed;
        rb.linearVelocity = velocity;
        
        // 检查是否触碰到墙或地面
        isTouchingLedge = player.CheckIfTouchingLedge();
        bool isTouchingWall = player.IsWallDetected();
        bool isTouchingGround = player.IsGroundDetected();
    
        // 如果触碰到墙但不是ledge，并且玩家还在下降态势时
        // Refine ledge detection; ensure ledge transitions are prioritized
        if (isTouchingWall && !isTouchingLedge && rb.linearVelocity.y < 0)
        {
            Debug.Log("Detected ledge, transitioning to LedgeClimbState.");
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
            stateMachine.ChangeState(player.ledgeClimbState);
        }

// Ensure wall slide only happens when actively sliding down a wall
        else if (isTouchingWall && rb.linearVelocity.y < 0 && Mathf.Abs(xDirection) > 0.1f)
        {
            Debug.Log("PlayerAirState: transitioning to WallSlideState.");
            stateMachine.ChangeState(player.wallSlideState);
        }

        if (isTouchingWall && !player.IsGroundDetected() && player.stateMachine.currentState != player.ledgeClimbState)
        {
            Debug.Log("change to wall slide");
            stateMachine.ChangeState(player.wallSlideState);
        }
        // 避免玩家卡住，仅在特定条件下切换为WallSlide状态
        if (isTouchingWall && rb.linearVelocity.y < 0 && Mathf.Abs(xDirection) > 0.1f)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }// Avoid overlapping conditions causing unintentional wall slide
        if (isTouchingWall && !isTouchingGround && rb.linearVelocity.y < 0 && Mathf.Abs(xDirection) > 0.1f)
        {
            Debug.Log("PlayerAirState: transitioning to WallSlideState");
            stateMachine.ChangeState(player.wallSlideState);
        }

// Prevent back wall detection affecting front movement
        if (player.isWallBackDetected() && player.rb.linearVelocity.y < 0 && Mathf.Abs(xDirection) < 0.1f)
        {
            // Prevent transition to states since the player isn’t actively moving or grounded
            Debug.Log("Back wall detected, but transition ignored.");
        }
        else if (player.isWallBackDetected() && player.rb.linearVelocity.y < 0 && Mathf.Abs(xDirection) > 0.1f)
        {
            // Transition logic if the player IS actively moving backward against the wall
            Debug.Log("Back wall detected, transitioning state.");
            stateMachine.ChangeState(player.wallSlideState);
        }
        if (!player.IsGroundDetected())
        {
            rb.linearVelocity += Vector2.down * (player.GetGravityMultiplier() * Time.deltaTime);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -player.GetMaxFallSpeed(), Mathf.Infinity));
            
        }else if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
        
    }
}