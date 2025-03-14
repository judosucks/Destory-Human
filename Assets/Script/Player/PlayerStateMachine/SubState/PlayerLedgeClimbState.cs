using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;
    private Vector2 workspace;
    private bool isClimbing;
    private new int xInput;
    private new int yInput;
    private bool jumpInput;
    private bool isTouchingCeiling;
    public PlayerLedgeClimbState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine,_playerData, _animBoolName)
    {
        
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        playerData.isHanging = true;
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.anim.SetBool("ClimbLedge", false);
    }
    public override void Enter()
    {
        base.Enter();
        
        if (LedgeTriggerDetection.isTouchingLedge)
        {
            rb.linearVelocity = Vector2.zero;

            // 设置玩家到达悬崖检测点（减去偏移）
            cornerPos = LedgeTriggerDetection.ledgePosition;

            startPos.Set(cornerPos.x - (player.facingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
            stopPos.Set(cornerPos.x + (player.facingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

            // 设置玩家位置
            player.transform.position = startPos;
            isClimbing = false; // 开始悬崖攀爬前的初始状态
            playerData.isLedgeClimbState = true; // 标记为悬崖攀爬状态
        }
        else
        {
            Debug.Log("Ledge Climbing State Entered without touching ledge!");
            stateMachine.ChangeState(player.idleState); // 如果没有检测到悬崖
        }


    }

    public override void Exit()
    {
        base.Exit();
        playerData.isHanging = false;
        if (isClimbing)
        {
            
            player.transform.position = stopPos;
            isClimbing = false;
        }
        playerData.isLedgeClimbState = false;
        
        // 启用玩家控制
        // player.SetIsBusy(false);
        // // // 重置悬崖攀爬动画
        // // player.anim.SetBool("isClimbing", false);
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            if (isTouchingCeiling)
            {
                Debug.LogWarning("touching ceiling fronm ledge climb");
                stateMachine.ChangeState(player.crouchIdleState);
            }
            else
            {
              stateMachine.ChangeState(player.idleState);
              Debug.LogWarning("idle from ledge climb");
            }
        }
        else
        {
            xInput = player.inputController.norInputX;
            yInput = player.inputController.norInputY;
            jumpInput = player.inputController.runJumpInput;
            
            rb.linearVelocity = Vector2.zero;
            player.transform.position = startPos;
            if (xInput == player.facingDirection && playerData.isHanging && !isClimbing)
            {
                CheckForSpace();
                isClimbing = true;
                player.anim.SetBool("ClimbLedge", true);
            }
            else if (yInput == -1 && playerData.isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.airState);
            }
            else if (jumpInput && !isClimbing)
            {
                player.wallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.wallJumpState);
            }
        }
        
    }
    public void SetDetectedPosition(Vector2 pos)
    {
        detectedPos = pos;
    }

    private bool CheckForSpace()
    {
        Debug.LogWarning("check for space");
        float capsuleRadius = playerData.standColliderSize.x / 2f;

        Vector2 rayOrigin = cornerPos 
                            + (Vector2.up * (capsuleRadius + playerData.ceilingCheckOffset))
                            + (Vector2.right * player.facingDirection * capsuleRadius);

        isTouchingCeiling = Physics2D.Raycast(
            rayOrigin,
            Vector2.up,
            playerData.standColliderSize.y - capsuleRadius,
            playerData.whatIsCeiling
        );

        Debug.Log("Is touching ceiling: " + isTouchingCeiling);
        player.anim.SetBool("IsTouchingCeiling", isTouchingCeiling);
        Debug.DrawRay(rayOrigin, Vector2.up * (playerData.standColliderSize.y - capsuleRadius), Color.red);

        return isTouchingCeiling;
    }
    
    public override void DoChecks()
    {
        base.DoChecks();
        
    }
}