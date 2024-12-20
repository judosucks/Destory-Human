using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;
    private bool isClimbing;
    private int xInput;
    private int yInput;
    public PlayerLedgeClimbState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        triggerCalled = true;
        Debug.Log("AnimationFinishTrigger player.ishanging"+player.isHanging);
        player.anim.SetBool("ClimbLedge", false);
    }
    public override void Enter()
    {
        base.Enter();
        player.ZeroVelocity();
        player.transform.position = detectedPos;
        cornerPos = player.DetermineCornerPosition();
        
        startPos.Set(cornerPos.x - (player.facingDirection * player.startOffset.x),cornerPos.y - player.startOffset.y);
        stopPos.Set(cornerPos.x+(player.facingDirection * player.stopOffset.x),cornerPos.y + player.stopOffset.y);
        player.transform.position = startPos;
    }

    public override void Exit()
    {
        base.Exit();
        player.isHanging = false;
        triggerCalled = false;
        if (isClimbing)
        {
            Debug.Log("isclimbing"+isClimbing);
            player.transform.position = stopPos;
            isClimbing = false;
        }
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
            Debug.Log("trigger called");
           
                
            stateMachine.ChangeState(player.idleState);
            Debug.Log("idle from ledge climb");
        }
        else
        {
            xInput = Mathf.RoundToInt(moveDirection);
            yInput = Mathf.RoundToInt(yDirection);
            Debug.Log(xInput+yInput+"xInput yinput");
            player.ZeroVelocity();
            player.transform.position = startPos;
            if (xInput == player.facingDirection && player.isHanging && !isClimbing)
            {
                isClimbing = true;
                player.anim.SetBool("ClimbLedge", true);
            }else if (yInput == -1 && player.isHanging && !isClimbing)
            {
                Debug.Log("yinput"+yInput);
                
                
                Debug.Log("statemachine is not null chaning state");    
                stateMachine.ChangeState(player.straightJumpAirState);
                
            }
        }
       
        
        // // 如果动画结束，移动角色到平台上
        // if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("EndOfClimb"))
        // {
        //     stateMachine.ChangeState(player.idleState);
        //     player.transform.position = player.climbOverPosition;
        // }
    }
    public void SetDetectedPosition(Vector2 pos)=> detectedPos = pos;
}

