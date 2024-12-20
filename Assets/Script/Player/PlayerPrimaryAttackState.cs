using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 0.5f;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        moveDirection = 0f; //we need this to fix bug on attack direction
        player.isAttacking = true;
        if (comboCounter > 4 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }
        player.anim.SetInteger("ComboCounter",comboCounter);
        float attackDirection = player.facingDirection;
        if (moveDirection != 0)
        {
            attackDirection = moveDirection;
        }
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection,player.attackMovement[comboCounter].y);
        stateTimer = .2f;
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor",.01f);
        comboCounter++;
        lastTimeAttacked = Time.time;
        player.isAttacking = false; // 设置攻击状态为 false
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            player.ZeroVelocity();
        }
        
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
        

    }
}
