using UnityEngine;

public class AngelMoveState : AngelGroundedState
{
   public AngelMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Angel _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
   {
      this.enemy = _enemy;
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
      enemy.SetVelocity(enemy.moveSpeed * enemy.facingDirection,rb.linearVelocity.y);
      if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
      {
         enemy.Flip();
         stateMachine.ChangeState(enemy.idleState);
      }
   }
}
