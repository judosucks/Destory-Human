using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallSlideState : PlayerState
{
    private bool isTouchingGround;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    private int xInput;
    private int yInput;

    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData,
        string _animBoolName) : base(_player,
        _stateMachine, _playerData, _animBoolName)
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

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        isTouchingGround = player.IsGroundDetected();
        isTouchingWall = player.IsWallDetected();
        isTouchingLedge = player.CheckIfTouchingLedge();
        xInput = player.inputController.norInputX;
        yInput = player.inputController.norInputY;
    }

    public override void Update()
    {
        base.Update();
        if (!isTouchingWall)
        {
            stateMachine.ChangeState(player.airState);
        }





        if (xInput != 0 && player.facingDirection == xInput)
        {
            // Debug.Log("change to air state xinput != 0 && player.facingDirection != xInput");
            // stateMachine.ChangeState(player.straightJumpAirState);
            rb.linearVelocity = new Vector2(0, 0);
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                stateMachine.ChangeState(player.wallJumpState);
                return;
            }
        }

        if (!isTouchingWall || xInput != player.facingDirection)
        {
            Debug.Log("change to air state !isTouchingWall || xInput != player.facingDirection");
            stateMachine.ChangeState(player.airState);
        }

        if (isTouchingGround)
        {
            stateMachine.ChangeState(player.idleState);
        }


        if (yInput > 0)
        {
            stateMachine.ChangeState(player.climbState);
        }
        else if (yInput < 0)
        {
            player.SetVelocityY(playerData.wallSlideDownForce);
        }
        else
        {
            player.SetVelocityY(player.CurrentVelocity.y * .7f);
        }

    }


}


