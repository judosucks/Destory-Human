using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CloneSkill : Skill
{
  [Header("Multi Clone")]
  [SerializeField] private float multiCloneAttackMultiplier;
  [SerializeField] private bool canDuplicateClone;
  [SerializeField] private float chanceToDuplicate;
  [Header("clone info")]
  [SerializeField]private GameObject clonePrefab;

  [SerializeField] private float attackMultiplier;
  [SerializeField] private float cloneDuration;
  [Space]
  [SerializeField] private bool canAttack;
  

  public void CreateClone(Transform _clonePosition,Vector3 _offset)
  {
    GameObject newClone = Instantiate(clonePrefab);
    newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition,cloneDuration,canAttack,_offset,canDuplicateClone,chanceToDuplicate,player,attackMultiplier);
  }
}
