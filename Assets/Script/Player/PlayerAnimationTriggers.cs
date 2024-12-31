using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    [SerializeField] private GameObject playerGameObject; // Ensure this is referenced correctly in the inspector.
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void DashEvent()
    {
        Debug.Log("dash event from animationtrigger");
        player.OnDashAttackFrame();
    }

    private void DashEndEvent()
    {
        player.OnDashAttackComplete();
    }

    private void CrossKickEvent()
    {
        player.OnPerformCrossKick();
    }

    private void CrossKickEndEvent()
    {
        player.OnCrossKickComplete();
    }
    private void AnimationTriggerClimbEvent()
    {
       
        player.isHanging = true;
    }
    private void AnimationFinishEvent()
    {
        
        player.ledgeClimbState.AnimationFinishTrigger();
    }

   
    
    

    

    private void AnimationLighteningTrigger()
    {
        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
        {
            var currentState = PlayerManager.instance.player.stateMachine.currentState;

            // Skip triggering if the skill is complete or state is transitioning
            if (PlayerManager.instance.player.skill.blackholeSkill.BlackholeSkillCompleted())
            {
                
                return;
            }

            if (currentState == PlayerManager.instance.player.blackholeState)
            {
                Debug.Log("lightning was fired from animationtrigger");
                player.GetComponent<EntityFX>().PlayerLightningFx(player.transform);
                
            }
            
        }
        
    }
    
    
    private void AttackTrigger()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (player.isCrossKick && hit.GetComponent<Enemy>()!=null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                // 应用 CrossKick 的击打逻辑
                Vector2 crossKickForce = new Vector2(player.specialKnockbackForce , player.firstKickKnockbackYdirection);
                hit.GetComponent<Enemy>().ApplyKnockback(crossKickForce);
                player.stats.DoDamage(_target);
                Debug.Log("enemy received crosskick knockback");
            }

            if (player.isKneeKick && hit.GetComponent<Enemy>()!=null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                Vector2 kneeKickForce = new Vector2(player.kneeKickKnockbackDirection.x,
                    player.kneeKickKnockbackDirection.y);
                hit.GetComponent<Enemy>().ApplyKnockback(kneeKickForce);
                player.stats.DoDamage(_target);
                Debug.Log("enemy received knee kick knockback");
                
            }
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(_target);
                
               
                
                

                
            }
        }
        
    }

    private void GrenadeThrownEvent()
    {
        Debug.Log("throw grenade event from animationtrigger");
        player.OnAimingStop();
        SkillManager.instance.grenadeSkill.CreateGrenade();
        
        OnAimCheckDecideToFalseEvent();
        if (player.anim.GetBool("AimGrenade"))
        {
            Debug.Log("aim grenade is true turn to false");
          player.anim.SetBool("AimGrenade",false);
          
        }
        // player.stateMachine.ChangeState(player.idleState);
        Debug.Log("grenade thrown is aiming should be false"+" "+player.isAiming);
    }

    private void GrenadeAimingEvent()
    {
       player.OnAimingStart();
       Debug.Log("[GrenadeAimingEvent] Aiming set to true");
    }

    private void OnGrendeCancelEndEvent()
    {
        Debug.Log("grenade cancel set to false");
        player.grenadeCanceled = false;
    }
    private void ForceToResetTrigger()
    {
        Debug.Log("force to reset trigger");
        player.anim.ResetTrigger("ThrowGrenade");
    }

    private void ForceToResetBool()
    {
        Debug.Log("force to reset bool");
        player.anim.SetBool("AimGrenade", false);
    }

    private void OnFinalCheckAiming()
    {
        if (player.isAiming||player.isAimCheckDecided||player.grenadeCanceled)
        {
            Debug.LogWarning("final check isaiming grenade canceled animatioin grenade cancel"+player.isAiming+player.grenadeCanceled+player.isAimCheckDecided);
            ForceToResetBool();
            ForceToResetTrigger();
            player.OnAimingStop();
            OnAimCheckDecideToFalseEvent();
            player.grenadeCanceled = false;
            
        }
       
    }

    private void OnGrenadeThrowComplete()
    {
        Debug.Log("grenade throw complete");
        StartCoroutine(player.BusyFor(1.0f));
    }

    

    // private void IsBusyIdleToGrenadeState()
    // {
    //     player.anim.SetBool("IsBusy", player.isBusy);
    // }
    private void OnAimCheckDecideToFalseEvent()
    {
        player.isAimCheckDecided = false;
        OnGrenadeThrowComplete();
    } 
    private void OnAimCheckDecideToTrueEvent()
    {
        player.isAimCheckDecided = true;
    }
    private void AnimationFinishTrigger()
    {
        if (player.stateMachine.currentState == player.idleState)
        {
            
            player.AnimationTrigger();
        }
    }
}
