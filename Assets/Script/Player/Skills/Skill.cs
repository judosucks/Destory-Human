using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    protected float cooldownTimer;
    protected Player player;
    [SerializeField]protected PlayerData playerData;
    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            Debug.Log("useskill from skill");
            //use skill
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        Debug.Log("skill is on cooldown");
        return false;
    }
    public virtual void UseSkill()
    {
        //do some skill stuff
        Debug.Log("skill used");
    }
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, playerData.closestEnemyCheckRadius,playerData.whatIsEnemy);
        
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
                
            }
        }

        return closestEnemy;
    }
}
