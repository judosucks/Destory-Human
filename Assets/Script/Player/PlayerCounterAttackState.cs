using Unity.VisualScripting;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine,_playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        stateTimer = playerData.counterAttackDuration;
        player.anim.SetBool("SuccessCounter",false);
        playerData.isCounterAttackState = true;
    }

    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, playerData.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    Debug.Log("enemy can be stunned");
                    stateTimer = 10f; //any value bigger than 1
                    player.anim.SetBool("SuccessCounter", true);
                    player.skill.parrySkill.UseSkill();
                    if (canCreateClone)
                    {
                      canCreateClone = false;  
                      player.skill.parrySkill.MakeMirageOnParry(hit.transform);
                      
                    }
                }
            }
        }
     
        if (stateTimer < 0 || triggerCalled)
        {
            Debug.Log("idle from counter");
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        playerData.isCounterAttackState = false;
    }
}
