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
    protected int xInput;
  
    protected int yInput;
    private string animBoolName;
    protected float startTime;
    protected bool triggerCalled;
    
    protected bool isExitingState;
    public PlayerState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.playerData = _playerData;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
     
       DoChecks();
       player.anim.SetBool(animBoolName, true);
       rb = player.rb;
       triggerCalled = false;
       
       startTime = Time.time;
       isExitingState = false;
       gamepad = Gamepad.current;
       mouse = Mouse.current;
       
       
    }

    public virtual void Update()
    {
        
        
       xInput = player.inputController.norInputX;
       yInput = player.inputController.norInputY;
      
       

    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void DoChecks()
    {
       
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
    public virtual void AnimationTrigger() { }
    
}