using UnityEngine;

public class AngelStunState : EnemyState
{
    private Enemy_Angel enemy;
    public AngelStunState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Angel _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.fx.InvokeRepeating("RedColorBlink", 0,.1f);
        stateTimer = enemy.stunDuration;
        rb.linearVelocity = new Vector2(-enemy.facingDirection * enemy.stunDirection.x,enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelColorChange",0);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
