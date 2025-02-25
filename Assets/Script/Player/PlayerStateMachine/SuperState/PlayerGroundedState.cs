
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerGroundedState : PlayerState
{
    private bool runJumpInput;
    private bool sprintJumpInput;
    private bool straightJumpInput;
    private bool isAirGrounded;
    private bool isStraightGrounded;
    protected new int xInput;
    protected new int yInput;
    private bool isTouchingWall;
    protected bool isTouchingLedge;
    private bool isTouchingHead;
    private bool isTouchingGround;
    protected bool isTouchingWallBack;
    private bool grabInput;
    protected bool isTouchingCeiling;
    private int oldXinput;
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    
    public override void Enter()
    {
        base.Enter();
        playerData.isGroundedState = true;

    }

    public override void Exit()
    {
        base.Exit();
        playerData.isGroundedState = false;
        
        isTouchingGround = false;
        isTouchingHead = false;
        isTouchingWall = false;
        isTouchingLedge = false;
        isTouchingWallBack = false;
        
        
    }

    

    public override void DoChecks()
    {
        base.DoChecks();

        isTouchingCeiling = player.CheckIfTouchingCeiling();
        isTouchingGround = player.IsGroundDetected();
        isTouchingWall = player.IsWallDetected();
        isTouchingLedge = player.CheckIfTouchingLedge();
        isTouchingHead = player.headDetection.isTouchingHead;
        isTouchingWallBack = player.isWallBackDetected();
            player.jumpState.ResetAmountOfJumps();
            

      



    }

    public override void Update()
    {
        base.Update();
        xInput = player.inputController.norInputX;
        yInput = player.inputController.norInputY;
        runJumpInput = player.inputController.runJumpInput;
        sprintJumpInput = player.inputController.sprintJumpInput;
        grabInput = player.inputController.grabInput;
        oldXinput = xInput;
        
        // if (!isExitingState && player.inputController.runJumpInput && player.jumpState.CanJump())
        // {
        //     Debug.Log("Switching to PlayerJumpState");
        //     stateMachine.ChangeState(player.jumpState);
        //     return;
        // }
        if (!playerData.isInAir&&!isTouchingGround && !player.isAttacking&&!playerData.isJumpState&& !playerData.isCounterAttackState&&!playerData.isBlackholeState && !playerData.isGrenadeState && !playerData.isSlopeClimbState)
        {
            player.startFallHeight = 0f;
            player.startFallHeight = player.transform.position.y;

            // Coyote Time 仅在特定条件下启动
            if (!playerData.isInAir)
            {
                player.airState.StartCoyoteTime();
            }

            Debug.LogWarning("isTouchingGround false from grounded state"+player.startFallHeight);
            stateMachine.ChangeState(player.airState);
            return;
        }
        

        if (runJumpInput && playerData.isRun&& player.jumpState.CanJump() && !player.isOnSlope || runJumpInput && playerData.isIdle && player.jumpState.CanJump()&& !player.isOnSlope)
        {
            playerData.highestPoint = player.transform.position.y;
            if (isTouchingHead) return;
            
            stateMachine.ChangeState(player.jumpState);
        }

        if (sprintJumpInput && playerData.isSprint&& player.jumpState.CanJump()&& !player.isOnSlope)
        {
            playerData.highestPoint = player.transform.position.y;
            
            if (isTouchingHead) return;
            stateMachine.ChangeState(player.sprintJumpState);
        }

        
        if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }

        
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            
            stateMachine.ChangeState(player.blackholeState);
        }

        if (mouse.rightButton.isPressed && playerData.rightButtonLocked)
        {
            return;
        }
    if (mouse.rightButton.isPressed && !player.grenade)
        {
            playerData.mouseButttonIsInUse = true;
            if (playerData.grenadeCanceled)
            {
                playerData.rightButtonLocked = true;
                return;
            }
            // player.anim.ResetTrigger("ThrowGrenade");
            
            
            playerData.rightButtonLocked = true;
            stateMachine.ChangeState(player.throwGrenadeState);
        }

        if (mouse.rightButton.wasReleasedThisFrame)
        {
            if (playerData.grenadeCanceled || !playerData.isAiming)
            {
                playerData.mouseButttonIsInUse = false;
                playerData.rightButtonLocked = false;
            }
            

        }
        
        if (Keyboard.current.qKey.wasPressedThisFrame && player.skill.parrySkill.parryUnlocked)
        {
            stateMachine.ChangeState(player.counterAttackState);
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && !playerData.isSlopeClimbState&& !playerData.isCrouchIdleState&&!playerData.isCrouchMoveState||(gamepad!=null && gamepad.buttonWest.wasPressedThisFrame)&& !playerData.isCrouchIdleState&& !playerData.isSlopeClimbState && !playerData.isCrouchMoveState)
        {
            if (playerData.mouseButttonIsInUse)
            {
                return;
            }
            stateMachine.ChangeState(player.primaryAttackState);
        }

        

      
        
        // if (!player.IsGroundDetected()&& xDirection != 0)
        // {
        //     stateMachine.ChangeState(player.airState);
        // }else if (!player.IsGroundDetected() && xDirection == 0)
        // {
        //     stateMachine.ChangeState(player.straightJumpAirState);
        // }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        // player.SlopeCheck();

        if ((xInput <= 0) && player.facingDirection == -1 && !player.isOnSlope) // 检测玩家朝左（或者未移动）
        {
            if (!player.leftEdgeTrigger.isNearLeftEdge) // 玩家不靠近左边缘
            {
                Debug.LogWarning("!leftEdgeTrigger: Player not near left edge");
        
                // 如果未触碰地面，玩家从边缘掉落
                player.isFallingFromEdge = !isTouchingGround;

                if (player.isFallingFromEdge)
                {
                    Debug.Log("Player is falling from edge!");
                    stateMachine.ChangeState(player.airState);
                }
            }
        }
        
        if ((xInput >= 0) && player.facingDirection == 1 && !player.isOnSlope) // 检测玩家朝right（或者未移动）
        {
            if (!player.rightEdgeTrigger.isNearRightEdge) // 玩家不靠近right边缘
            {
                Debug.LogWarning("!rightEdgeTrigger: Player not near right edge");
        
                // 如果未触碰地面，玩家从边缘掉落
                player.isFallingFromEdge = !isTouchingGround;

                if (player.isFallingFromEdge)
                {
                    Debug.Log("Player is falling from edge!");
                    stateMachine.ChangeState(player.airState);
                }
            }
        }
        
    }

    private bool HasNoGrenade()
    {
        if (!player.grenade)
        {
            Debug.Log("No grenade");
            return true;
        }
        Debug.Log("Grenade is not empty");
        player.skill.grenadeSkill.GetComponent<GrenadeSkillController>().ReadyToUseGrenade();
        return false;
    }
}