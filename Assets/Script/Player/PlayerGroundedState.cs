//
// using UnityEngine;
// using UnityEngine.InputSystem;
//
//
// public class PlayerGroundedState : PlayerState
// {
//     private bool mouseButttonIsInUse;
//     public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
//         _stateMachine, _animBoolName)
//     {
//         
//     }
//
//     public override void Enter()
//     {
//         base.Enter();
//     }
//
//     public override void Exit()
//     {
//         base.Exit();
//     }
//
//     public override void Update()
//     {
//         base.Update();
//
//
//         moveDirection = Input.GetAxisRaw("Horizontal");
//         if (Keyboard.current.rKey.wasPressedThisFrame)
//         {
//             Debug.Log("blackhole");
//             stateMachine.ChangeState(player.blackholeState);
//         }
//
//         if (mouse.rightButton.isPressed && player.rightButtonLocked)
//         {
//             Debug.Log("right button locked");
//             return;
//         }
//     if (mouse.rightButton.isPressed)
//         {
//             mouseButttonIsInUse = true;
//             Debug.Log("right mouse button pressed from grounded state");
//             if (player.grenadeCanceled && player.skill.grenadeSkill.rightButtonIsPressed)
//             {
//                 Debug.Log("Grenade canceled abort");
//                 player.rightButtonLocked = true;
//                 
//                 
//                 return;
//             }
//             // player.anim.ResetTrigger("ThrowGrenade");
//             
//             stateMachine.ChangeState(player.throwGrenadeState);
//             player.rightButtonLocked = true;
//         }
//
//         if (mouse.rightButton.wasReleasedThisFrame)
//         {
//             mouseButttonIsInUse = false;
//             player.rightButtonLocked = false;
//             Debug.Log("right mouse button released from grounded state player.rightbuttonlocked"+player.rightButtonLocked);
//
//         }
//         
//         if (Keyboard.current.qKey.wasPressedThisFrame)
//         {
//             Debug.Log("Q pressed counter attack from grounded state");
//             stateMachine.ChangeState(player.counterAttackState);
//         }
//         if (Mouse.current.leftButton.wasPressedThisFrame && !mouseButttonIsInUse ||(gamepad!=null && gamepad.buttonWest.wasPressedThisFrame)&& !mouseButttonIsInUse)
//         {
//             Debug.Log("mousebuttonisinuse"+mouseButttonIsInUse);
//             stateMachine.ChangeState(player.primaryAttackState);
//         }
//         
//         if (!player.IsGroundDetected()&& moveDirection != 0)
//         {
//             stateMachine.ChangeState(player.airState);
//         }else if (!player.IsGroundDetected() && moveDirection == 0)
//         {
//             stateMachine.ChangeState(player.straightJumpAirState);
//         }
//         if ((gamepad != null && gamepad.buttonSouth.wasPressedThisFrame && player.IsGroundDetected() && moveDirection != 0) || Keyboard.current.spaceKey.wasPressedThisFrame && player.IsGroundDetected() && moveDirection != 0)
//         {
//             stateMachine.ChangeState(player.jumpState);
//         }else if ((gamepad != null && gamepad.buttonSouth.wasPressedThisFrame && player.IsGroundDetected() && moveDirection == 0) || Keyboard.current.spaceKey.wasPressedThisFrame && player.IsGroundDetected() && moveDirection == 0)
//         {
//             stateMachine.ChangeState(player.straightJumpState);
//         }
//     }
//
//     private bool HasNoGrenade()
//     {
//         if (!player.grenade)
//         {
//             Debug.Log("No grenade");
//             return true;
//         }
//         Debug.Log("Grenade is not empty");
//         player.skill.grenadeSkill.GetComponent<GrenadeSkillController>().ReadyToUseGrenade();
//         return false;
//     }
// }
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerGroundedState : PlayerState
{
    
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

    public override void Update()
    {
        base.Update();


        xDirection = Input.GetAxisRaw("Horizontal");
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("blackhole");
            stateMachine.ChangeState(player.blackholeState);
        }

        if (mouse.rightButton.isPressed && player.rightButtonLocked)
        {
            Debug.Log("right button locked");
            return;
        }
    if (mouse.rightButton.isPressed && !player.grenade)
        {
            player.mouseButttonIsInUse = true;
            Debug.Log("right mouse button pressed from grounded state");
            if (player.grenadeCanceled)
            {
                Debug.Log("Grenade canceled abort");
                player.rightButtonLocked = true;
                
                
                return;
            }
            // player.anim.ResetTrigger("ThrowGrenade");
            
            
            player.rightButtonLocked = true;
            stateMachine.ChangeState(player.throwGrenadeState);
        }

        if (mouse.rightButton.wasReleasedThisFrame)
        {
            if (player.grenadeCanceled || !player.isAiming)
            {
              player.mouseButttonIsInUse = false;
              player.rightButtonLocked = false;
              Debug.Log("right mouse button released from grounded state"+player.rightButtonLocked);
            }
            

        }
        
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("Q pressed counter attack from grounded state");
            stateMachine.ChangeState(player.counterAttackState);
        }
        if (Mouse.current.leftButton.wasPressedThisFrame||(gamepad!=null && gamepad.buttonWest.wasPressedThisFrame))
        {
            if (player.mouseButttonIsInUse)
            {
                Debug.Log("mouse is in use");
                return;
            }
            stateMachine.ChangeState(player.primaryAttackState);
        }
        
        if (!player.IsGroundDetected()&& xDirection != 0)
        {
            stateMachine.ChangeState(player.airState);
        }else if (!player.IsGroundDetected() && xDirection == 0)
        {
            stateMachine.ChangeState(player.straightJumpAirState);
        }
        if ((gamepad != null && gamepad.buttonSouth.wasPressedThisFrame && player.IsGroundDetected() && xDirection != 0) || Keyboard.current.spaceKey.wasPressedThisFrame && player.IsGroundDetected() && xDirection != 0)
        {
            stateMachine.ChangeState(player.jumpState);
        }else if ((gamepad != null && gamepad.buttonSouth.wasPressedThisFrame && player.IsGroundDetected() && xDirection == 0) || Keyboard.current.spaceKey.wasPressedThisFrame && player.IsGroundDetected() && xDirection == 0)
        {
            stateMachine.ChangeState(player.straightJumpState);
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