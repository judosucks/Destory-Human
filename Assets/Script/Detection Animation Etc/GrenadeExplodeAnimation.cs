using System;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeExplodeAnimation : MonoBehaviour
{
    private Player player;
    private GrenadeSkillController grenadeSkillController;

    private void Start()
    {
        player = PlayerManager.instance.player;
        grenadeSkillController = GetComponentInParent<GrenadeSkillController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            Debug.Log("detect enemy");
            EnemyStats target = other.GetComponent<EnemyStats>();
            if (target != null)
            {
                Debug.LogWarning("trigger target"+target.currentHealth);
              player.stats.DoDamage(target);
            }
            else
            {
                Debug.LogWarning("no enemystats foudn on the collider object");
            }
        }
    }


    private void OnExplosionEffectComplete()
    {
        Debug.Log("destroying grenade");
        Destroy(gameObject);
        
    }
    
    
}
