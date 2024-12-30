using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;


public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Mouse mouse;
    protected Rigidbody2D rb;
    protected Gamepad gamepad;
    protected float moveDirection;
    protected float yDirection;
    private string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;
    protected bool canPerformDashAttack; // flag to check if dash attack is allowed
    
    
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
       player.anim.SetBool(animBoolName, true);
       rb = player.rb;
       triggerCalled = false;
       canPerformDashAttack = false;
       
       gamepad = Gamepad.current;
       mouse = Mouse.current;
       
    }

    public virtual void Update()
    {
        
        stateTimer -= Time.deltaTime;
       moveDirection = Input.GetAxisRaw("Horizontal");
       yDirection = Input.GetAxisRaw("Vertical");
       player.anim.SetFloat("yVelocity", rb.linearVelocity.y);
       
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
    

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    public virtual void CanPerformDashAttack()
    {
        canPerformDashAttack = true;
    }

    public virtual void CanNotPerformDashAttack()
    {
        canPerformDashAttack = false;
    }
    public virtual void PerformCrossKick()
    {
        player.isCrossKick = true; //执行crosskick动作
        Debug.Log("Player is cross kicking"+player.isCrossKick);
    }

    public virtual void PerformRegularAttack()
    {
        player.isCrossKick = false;// 执行普通攻击动作
        
        Debug.Log("player finish crosskick"+player.isCrossKick);
    }

    
    

    
    
   
}