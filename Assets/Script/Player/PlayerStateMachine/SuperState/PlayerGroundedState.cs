using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerState
{
    private bool runJumpInput;
    private bool sprintJumpInput;
    protected new int xInput;
    protected new int yInput;
    private bool isTouchingWall;
    protected bool isTouchingLedge;
    private bool isTouchingHead;
    private bool isTouchingGround;
    protected bool isTouchingWallBack;
    private bool grabInput;
    protected bool isTouchingCeiling;
    protected bool isTouchingWallBottom;
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
        isTouchingLedge = LedgeTriggerDetection.isTouchingLedge;
        isTouchingHead = player.CheckIfTouchingHead();
        isTouchingWallBack = player.IsWallBackDetected();
        isTouchingWallBottom = player.IsWallBottomDetected();
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
        
        // if the player is not on the ground transition to air state
        if (!isTouchingGround && !player.isAttacking && !playerData.isCounterAttackState && !playerData.isBlackholeState && !playerData.isGrenadeState && !playerData.isSlopeClimbState)
        {
            player.startFallHeight = player.transform.position.y;
            if (!player.airState.isCoyoteTimeTriggered)
            {
               Debug.Log("startcoyotetime");
               player.airState.StartCoyoteTime();
               player.airState.isCoyoteTimeTriggered = true;
            }
            Debug.LogWarning("isTouchingGround false from grounded state"+player.startFallHeight);
            stateMachine.ChangeState(player.airState);
            return;
        }
        

        if (runJumpInput && playerData.isRun && player.jumpState.CanJump() && !player.isOnSlope || runJumpInput && playerData.isIdle && player.jumpState.CanJump() && !player.isOnSlope)
        {
            playerData.highestPoint = player.transform.position.y;
            if (isTouchingHead) return;
            stateMachine.ChangeState(player.jumpState);
            return;
        }

        if (sprintJumpInput && playerData.isSprint && player.jumpState.CanJump() && !player.isOnSlope)
        {
            playerData.highestPoint = player.transform.position.y;
            if (isTouchingHead) return;
            stateMachine.ChangeState(player.sprintJumpState);
            return;
        }

        
        if (isTouchingWall && grabInput)
        {
            stateMachine.ChangeState(player.wallGrabState);
            return;
        }

        
        if (Keyboard.current.rKey.wasPressedThisFrame && player.skill.blackholeSkill.unlockBlackhole)
        {
            stateMachine.ChangeState(player.blackholeState);
            return;
        }

        if (mouse.rightButton.isPressed && playerData.rightButtonLocked)
        {
            return;
        }
    if (mouse.rightButton.isPressed && !player.grenade && player.skill.grenadeSkill.grenadeUnlocked)
        {
            playerData.mouseButttonIsInUse = true;
            if (playerData.grenadeCanceled)
            {
                playerData.rightButtonLocked = true;
                return;
            }
            
            playerData.rightButtonLocked = true;
            stateMachine.ChangeState(player.throwGrenadeState);
            return;
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
            return;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && !playerData.isSlopeClimbState && !playerData.isCrouchIdleState && !playerData.isCrouchMoveState || (gamepad != null && gamepad.buttonWest.wasPressedThisFrame) && !playerData.isCrouchIdleState && !playerData.isSlopeClimbState && !playerData.isCrouchMoveState)
        {
            if (playerData.mouseButttonIsInUse)
            {
                return;
            }
            stateMachine.ChangeState(player.primaryAttackState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // if close to the edge and not on the ground fall
        if (((xInput <= 0) && player.facingDirection == -1 && !player.isOnSlope) || ((xInput >= 0) && player.facingDirection == 1 && !player.isOnSlope))
        {
            if ((xInput <= 0 && !player.leftEdgeTrigger.isNearLeftEdge) || (xInput >= 0 && !player.rightEdgeTrigger.isNearRightEdge))
            {
                player.isFallingFromEdge = !isTouchingGround;
                if (player.isFallingFromEdge)
                {
                    if (isTouchingWallBack)
                    {
                        Debug.Log("wallback from grounded fallingfrom edge");
                       player.MoveTowardSmooth(playerData.moveDirection * player.facingDirection,playerData.moveAlittleDistance);
                       stateMachine.ChangeState(player.airState);
                       return;
                    }
                    else if (isTouchingWallBottom)
                    {
                        Debug.Log("wallbottom from grounded fallingfrom edge");
                        player.MoveTowardSmooth(playerData.moveDirection * -player.facingDirection, playerData.moveAlittleDistance);
                        stateMachine.ChangeState(player.airState);
                        return;
                    }
                }
            }
        }
    }
}