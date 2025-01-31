
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerGroundedState : PlayerState
{
    private bool runJumpInput;
    private bool sprintJumpInput;
    private bool straightJumpInput;
    private bool isAirGrounded;
    private bool isStraightGrounded;
    private int xInput;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    private bool isTouchingHead;
    private bool isTouchingGround;
    private bool isTouchingWallBack;
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    
    public override void Enter()
    {
        base.Enter();
        
        
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
        
        xInput = player.inputController.norInputX;
        isTouchingGround = player.IsGroundDetected();
        isTouchingWall = player.IsWallDetected();
        isTouchingLedge = player.CheckIfTouchingLedge();
        isTouchingHead = player.headDetection.isTouchingHead;
        isTouchingWallBack = player.isWallBackDetected();
        runJumpInput = player.inputController.runJumpInput;
        sprintJumpInput = player.inputController.sprintJumpInput;
        straightJumpInput = player.inputController.straightJumpInput;
        
        
    }

    public override void Update()
    {
        base.Update();
        
        
        
    
        if (runJumpInput && playerData.isRun && isTouchingGround)
        {
            
            player.inputController.UseRunJumpInput();
            playerData.highestPoint = player.transform.position.y;
            if (isTouchingHead) return;
            
            stateMachine.ChangeState(player.jumpState);
        }

        if (sprintJumpInput && playerData.isSprint && isTouchingGround)
        {
        
            player.inputController.UseSprintJumpInput();
            playerData.highestPoint = player.transform.position.y;
            
            if (isTouchingHead) return;
            stateMachine.ChangeState(player.sprintJumpState);
        }

        if (straightJumpInput && playerData.isIdle && isTouchingGround)
        {
            
            player.inputController.UseStraightJumpInput();
            playerData.highestPoint = player.transform.position.y;
            
            if (isTouchingHead)return;
            
            stateMachine.ChangeState(player.straightJumpState);
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
        if (Mouse.current.leftButton.wasPressedThisFrame||(gamepad!=null && gamepad.buttonWest.wasPressedThisFrame))
        {
            if (playerData.mouseButttonIsInUse)
            {
                return;
            }
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if (!isTouchingGround && xInput != 0 && !player.isAttacking&&!playerData.isJumpState&& !playerData.isCounterAttackState)
        {
            player.startFallHeight = 0f;
            player.startFallHeight = player.transform.position.y;
            stateMachine.ChangeState(player.airState);
        }

        if (!isTouchingGround && xInput == 0 && !player.isAttacking&&!playerData.isStraightJumpState && !playerData.isCounterAttackState)
        {
            player.startFallHeight = 0f;
            player.startFallHeight = player.transform.position.y;
            stateMachine.ChangeState(player.straightJumpAirState);
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
        
        if (xInput < 0 && player.facingDirection == -1)
        {
            if (!player.leftEdgeTrigger.isNearLeftEdge)
            {
                if (!isTouchingGround)
                {
                    player.isFallingFromEdge = true;
                    stateMachine.ChangeState(player.airState);
                }
                else
                {
                    player.isFallingFromEdge = false;
                }
            }
        }

        if (xInput > 0 && player.facingDirection == 1)
        {
            if (!player.rightEdgeTrigger.isNearRightEdge)
            {
                if (!isTouchingGround)
                {
                    player.isFallingFromEdge = true;
                    stateMachine.ChangeState(player.airState);
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