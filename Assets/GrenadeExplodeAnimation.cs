using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplodeAnimation : MonoBehaviour
{
    private Player player;
    private GrenadeSkillController grenadeSkillController;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        grenadeSkillController = GetComponentInParent<GrenadeSkillController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (grenadeSkillController != null)
        {
            grenadeSkillController.OnChildTriggerEnter2D(other);
        }
    }

    public void Test()
    {
        Debug.Log("test");
    }
    
}
