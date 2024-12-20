using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Enemy : Entity
{
   [Header("stun info")] 
   public float stunDuration;
   public Vector2 stunDirection;
   protected bool canBeStunned;
   [SerializeField] protected GameObject counterImage;
   [SerializeField]protected LayerMask whatIsPlayer;
   [Header("move info")] 
   private float defaultMoveSpeed;
   public float moveSpeed;
   public float idleTime;
   public float battleTime;
   [Header("attack info")] 
   public float attackDistance;

   public float attackCooldown;
   [HideInInspector] public float lastTimeAttacked;
   public EnemyStateMachine stateMachine { get; private set; }
   public string lastAnimBoolName { get; private set; }
   protected override void Awake()
   {
      base.Awake();
      stateMachine = new EnemyStateMachine();
      defaultMoveSpeed = moveSpeed;
   }

   protected override void Update()
   {
      base.Update();
      // 检查 stateMachine 和 currentState 是否为 null
      if (stateMachine != null && stateMachine.currentState != null)
      {
         stateMachine.currentState.Update();
      }
      else
      {
         Debug.LogWarning("State machine or current state is not initialized.");
      }
      var playerHit = IsPlayerDetected();

      // 检查 RaycastHit2D 的 collider 是否为 null
      if (playerHit.collider != null)
      {
         
      }
      else
      {
         
      }
      
   }

   public virtual void AssignLastAnimName(string _animBoolName)
   {
      lastAnimBoolName = _animBoolName;
   }
   public virtual RaycastHit2D IsPlayerDetected()=>Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 50, whatIsPlayer);

   protected override void OnDrawGizmos()
   {
      base.OnDrawGizmos();
      Gizmos.color = Color.red;
      Gizmos.DrawLine(transform.position,new Vector3(transform.position.x + attackDistance * facingDirection,transform.position.y));
   }

   public virtual void FreezeTime(bool _timeFreeze)
   {
      if (_timeFreeze)
      {
         moveSpeed = 0;
         anim.speed = 0;
      }
      else
      {
         moveSpeed = defaultMoveSpeed;
         anim.speed = 1;
      }
      
   }

   protected virtual IEnumerator FreezeTimerFor(float _seconds)
   {
      FreezeTime(true);
      yield return new WaitForSeconds(_seconds);
      FreezeTime(false);
   }

   public void ApplyKnockback(Vector2 force)
   {
      rb.linearVelocity = new Vector2(force.x * - facingDirection, force.y );
   }

   public virtual void OpenCounterAttackWindow()
   {
      canBeStunned = true;
      counterImage.SetActive(true);
   }

   public virtual void CloseCounterAttackWindow()
   {
      canBeStunned = false;
      counterImage.SetActive(false);
   }

   public virtual bool CanBeStunned()
   {
      if (canBeStunned)
      {
         CloseCounterAttackWindow();
         return true;
      }
      return false;
   }
   public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
