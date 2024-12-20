
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
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
        
        
        moveDirection = Input.GetAxisRaw("Horizontal");
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("blackhole");
            stateMachine.ChangeState(player.blackholeState);
        }
        if (mouse.rightButton.isPressed && !player.grenade)
        {
            Debug.Log("Right mouse button pressed no grenade");
            stateMachine.ChangeState(player.throwGrenadeState);
        }
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("Q pressed counter attack from grounded state");
            stateMachine.ChangeState(player.counterAttackState);
        }
        if (Mouse.current.leftButton.wasPressedThisFrame||(gamepad!=null && gamepad.buttonWest.wasPressedThisFrame))
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }
        
        if (!player.IsGroundDetected()&& moveDirection != 0)
        {
            stateMachine.ChangeState(player.airState);
        }else if (!player.IsGroundDetected() && moveDirection == 0)
        {
            stateMachine.ChangeState(player.straightJumpAirState);
        }
        if ((gamepad != null && gamepad.buttonSouth.wasPressedThisFrame && player.IsGroundDetected() && moveDirection != 0) || Keyboard.current.spaceKey.wasPressedThisFrame && player.IsGroundDetected() && moveDirection != 0)
        {
            stateMachine.ChangeState(player.jumpState);
        }else if ((gamepad != null && gamepad.buttonSouth.wasPressedThisFrame && player.IsGroundDetected() && moveDirection == 0) || Keyboard.current.spaceKey.wasPressedThisFrame && player.IsGroundDetected() && moveDirection == 0)
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
        // player.grenade.GetComponent<GrenadeSkillController>().ReturnGrenade();
        return false;
    }
}
