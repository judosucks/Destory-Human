
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerGroundedState : PlayerState
{
    private bool runJumpInput;
    private bool sprintJumpInput;
    private bool straightJumpInput;
    private bool sprintInput;
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
    }

    public override void Update()
    {
        base.Update();
        runJumpInput = player.inputController.runJumpInput;
        sprintJumpInput = player.inputController.sprintJumpInput;
        straightJumpInput = player.inputController.straightJumpInput;
        sprintInput = player.inputController.sprintInput;
        
        
        if (runJumpInput && playerData.isRun)
        {
            Debug.Log("runjumpinput");
            player.inputController.UseRunJumpInput();
            
            if (isTouchingHead) return;
            
            stateMachine.ChangeState(player.jumpState);
        }

        if (sprintJumpInput && playerData.isSprint)
        {
            Debug.Log("sprintjumpinput");
            player.inputController.UseSprintJumpInput();
            stateMachine.ChangeState(player.sprintJumpState);
        }

        if (straightJumpInput && playerData.isIdle)
        {
            
            player.inputController.UseStraightJumpInput();
            
            if (isTouchingHead)return;
            
            stateMachine.ChangeState(player.straightJumpState);
        }

        if (sprintInput)
        {
            player.inputController.UseSprintInput();
            stateMachine.ChangeState(player.sprintState);
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("blackhole");
            stateMachine.ChangeState(player.blackholeState);
        }

        if (mouse.rightButton.isPressed && playerData.rightButtonLocked)
        {
            Debug.Log("right button locked");
            return;
        }
    if (mouse.rightButton.isPressed && !player.grenade)
        {
            playerData.mouseButttonIsInUse = true;
            Debug.Log("right mouse button pressed from grounded state");
            if (playerData.grenadeCanceled)
            {
                Debug.Log("Grenade canceled abort");
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
              Debug.Log("right mouse button released from grounded state"+playerData.rightButtonLocked);
            }
            

        }
        
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("Q pressed counter attack from grounded state");
            stateMachine.ChangeState(player.counterAttackState);
        }
        if (Mouse.current.leftButton.wasPressedThisFrame||(gamepad!=null && gamepad.buttonWest.wasPressedThisFrame))
        {
            if (playerData.mouseButttonIsInUse)
            {
                Debug.Log("mouse is in use");
                return;
            }
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if (!isTouchingGround && xInput != 0)
        {
            stateMachine.ChangeState(player.airState);
        }

        if (!isTouchingGround && xInput == 0)
        {
            stateMachine.ChangeState(player.straightJumpAirState);
        }

        if (xInput < 0 && isTouchingGround)
        {
            Debug.LogWarning(xInput + " is less than 0");
            if (!player.leftEdgeTrigger.isNearLeftEdge)
            {
                Debug.Log("player is touching left edge");
                if (!isTouchingGround)
                {
                    Debug.Log("not touching ground left edge");
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                    player.CheckForCurrentVelocity();
                    if (isTouchingWallBack)
                    {
                        Debug.Log("is touching wall back left");
                        if (stateMachine.currentState != player.wallSlideState)
                        {
                            stateMachine.ChangeState(player.wallSlideState);
                        }
                    }

                }
            }
        }

        if (xInput > 0 && isTouchingGround)
        {
            Debug.LogWarning(xInput + " is less than 0");
            if (!player.rightEdgeTrigger.isNearRightEdge)
            {
                Debug.Log("player is touching right edge");
                if (!isTouchingGround)
                {
                    Debug.Log("not touching ground right edge");
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                    player.CheckForCurrentVelocity();
                    if (isTouchingWallBack)
                    {
                        Debug.Log("is touching wall back right");
                        if (stateMachine.currentState != player.wallSlideState)
                        {
                            stateMachine.ChangeState(player.wallSlideState);
                        }
                    }

                }
            }
        }
        // if (!player.IsGroundDetected()&& xDirection != 0)
        // {
        //     stateMachine.ChangeState(player.airState);
        // }else if (!player.IsGroundDetected() && xDirection == 0)
        // {
        //     stateMachine.ChangeState(player.straightJumpAirState);
        // }
        
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