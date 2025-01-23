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
    protected PlayerData playerData;
    protected Rigidbody2D rb;
    protected Gamepad gamepad;
    protected float xDirection;

    public float GetXDirection()
    {
      float value = xDirection;
      return value;
    }
    protected float yDirection;
    private string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;
    protected bool canPerformDashAttack; // flag to check if dash attack is allowed
    protected bool isAbilityDone;
    
    public PlayerState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.playerData = _playerData;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
     
        player.CheckForCurrentVelocity();
       player.anim.SetBool(animBoolName, true);
       rb = player.rb;
       triggerCalled = false;
       canPerformDashAttack = false;
       isAbilityDone = false;
       gamepad = Gamepad.current;
       mouse = Mouse.current;
       
    }

    public virtual void Update()
    {
        
        stateTimer -= Time.deltaTime;
       xDirection = Mathf.RoundToInt(player.inputController.norInputX);
       yDirection = Mathf.RoundToInt(player.inputController.norInputY);
       player.anim.SetFloat("yVelocity", player.CurrentVelocity.y);
       
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void DoChecks()
    {
        // if ( rb.linearVelocity.y < 0 && player.isWallBackDetected())
        // {
        //     Debug.Log("player is stuck");
        //     Vector2 correctionDirection = Vector2.up;//push up ward
        //     rb.position += correctionDirection * 0.1f;
        // }
    }

    public virtual void PhysicsUpdate()
    {
        player.CheckForCurrentVelocity();
        DoChecks();
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