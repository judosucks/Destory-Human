using System.Data.Common;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;

public class PlayerStandState : PlayerState
{
    private bool isGrounded;
    private bool isFrontBottomCheck;

    public PlayerStandState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData,
        string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerData.reachedApex = false;
        Debug.Log("enter stand state"+player.rightGroundDetected+" "+player.leftGroundDetected);
        player.SetVelocity(1f * player.facingDirection, 1f);
        if (player.rightGroundDetected && player.facingDirection ==1 || player.leftGroundDetected&& player.facingDirection == -1)
        {
            Debug.Log("not left right grounded from enter");
            if (!player.isGroundDetected)
            {
                Debug.Log("not grounded moveforward enter");
                player.MoveTowardSmooth(playerData.moveDirection * player.facingDirection,playerData.moveDistance);
                
            }

            
        }

        if (player.isGroundDetected)
        {
            Debug.Log("grounded snap to grid enter");
            player.SnapToGridSize(playerData.gridSize);
            rb.AddForce(Vector2.down * playerData.stickingForce,ForceMode2D.Impulse);
        }
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        isGrounded = false;
        isFrontBottomCheck = false;

    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.IsGroundDetected();
        isFrontBottomCheck = player.IsFrontBottomDetected();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (player.IsGroundDetected())
        {
            Debug.Log("grounded snap to grid");
            player.SnapToGridSize(playerData.gridSize);
            rb.AddForce(Vector2.down * playerData.stickingForce,ForceMode2D.Impulse);
            
        }
        if (!isGrounded && player.IsRightGroundDetected() || !isGrounded && player.IsLeftGroundDetected())
        {
            Debug.Log("not grounded moveforward right left");
            player.MoveTowardSmooth(playerData.moveDirection * player.facingDirection,playerData.moveDistance);
            if (player.IsGroundDetected())
            {
                Debug.Log("grounded");
                player.SnapToGridSize(playerData.gridSize);
                rb.AddForce(Vector2.down * playerData.stickingForce,ForceMode2D.Impulse);
            }
        }
        if (!player.IsRightGroundDetected()&& player.facingDirection == 1 ||  player.facingDirection == -1 && !player.IsLeftGroundDetected())
        { 
            if (!isGrounded && player.IsFrontBottomDetected() || player.isFallingFromEdge && !isGrounded && player.IsFrontBottomDetected())
            {
                Debug.Log("isfalling");
                
                if (Mathf.RoundToInt(player.CurrentVelocity.x) != 0)
                {
                    Debug.Log("currentvelocity!=0"+player.CurrentVelocity.x);
                  stateMachine.ChangeState(player.airState);
                }
                else if (Mathf.RoundToInt(player.CurrentVelocity.x) == 0)
                {
                    Debug.Log("currentvelocity"+player.CurrentVelocity.x);
                    stateMachine.ChangeState(player.straightJumpAirState);
                }
            }
            
        }
        
        
        // else if (isGrounded)
        // {
        //     Debug.Log("grounded");
        //     player.SnapToGrid();
        //     // rb.AddForce(Vector2.down * playerData.stickingForce,ForceMode2D.Impulse);
        // }
        
    }
}
