
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
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    
    public override void Enter()
    {
        base.Enter();
        player.jumpState.ResetAmountOfJumps();
        
    }

    public override void Exit()
    {
        base.Exit();
        
        
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
        player.CheckGroundStatus();




    }

    public override void Update()
    {
        base.Update();
        xInput = player.inputController.norInputX;
        yInput = player.inputController.norInputY;
        runJumpInput = player.inputController.runJumpInput;
        sprintJumpInput = player.inputController.sprintJumpInput;
        grabInput = player.inputController.grabInput;

        if (runJumpInput && playerData.isRun&& player.jumpState.CanJump() && !player.isOnSlope && !player.canWalkOnSlope || runJumpInput && playerData.isIdle && player.jumpState.CanJump()&& !player.isOnSlope && !player.canWalkOnSlope)
        {
            playerData.highestPoint = player.transform.position.y;
            if (isTouchingHead) return;
            
            stateMachine.ChangeState(player.jumpState);
        }

        if (sprintJumpInput && playerData.isSprint&& player.jumpState.CanJump()&& !player.isOnSlope && !player.canWalkOnSlope)
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
        if (Mouse.current.leftButton.wasPressedThisFrame && !playerData.isCrouchIdleState&&!playerData.isCrouchMoveState||(gamepad!=null && gamepad.buttonWest.wasPressedThisFrame)&& !playerData.isCrouchIdleState && !playerData.isCrouchMoveState)
        {
            if (playerData.mouseButttonIsInUse)
            {
                return;
            }
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if (!isTouchingGround && !player.isAttacking&&!playerData.isJumpState&& !playerData.isCounterAttackState&&!playerData.isBlackholeState && !playerData.isGrenadeState)
        {
            player.startFallHeight = 0f;
            player.startFallHeight = player.transform.position.y;
            player.airState.StartCoyoteTime();
            Debug.LogWarning("isTouchingGround false from grounded state"+player.startFallHeight);
            stateMachine.ChangeState(player.airState);
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
        
        if (xInput <= 0 && player.facingDirection == -1)
        {
            if (!player.leftEdgeTrigger.isNearLeftEdge)
            {
                Debug.LogWarning("!left edgetrigger");
                if (!isTouchingGround)
                {
                    player.isFallingFromEdge = true;
                    
                }
                else
                {
                    player.isFallingFromEdge = false;
                }
            }
        }
        
        if (xInput >= 0 && player.facingDirection == 1)
        {
            if (!player.rightEdgeTrigger.isNearRightEdge)
            {
                Debug.LogWarning("!right edgetrigger");
                if (!isTouchingGround)
                {
                    player.isFallingFromEdge = true;
                    
                }
                else
                {
                    player.isFallingFromEdge = false;
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