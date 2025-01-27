using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloneSkill : Skill
{
  [Header("Multi Clone")] [SerializeField]
  private float multiCloneAttackMultiplier;

  [SerializeField] private bool canDuplicateClone;
  [SerializeField] private float chanceToDuplicate;

  [Header("clone info")] [SerializeField]
  private GameObject clonePrefab;

  [SerializeField] private float attackMultiplier;
  [SerializeField] private float cloneDuration;
  [Space] [SerializeField] private bool canAttack;

  [SerializeField] private bool createCloneOnDashStart;
  [SerializeField] private bool createCloneOnDashOver;
  [SerializeField] private bool canCreateCloneOnCounterAttack;
  public void CreateClone(Transform _clonePosition, Vector3 _offset)
  {
    GameObject newClone = Instantiate(clonePrefab);
    newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset,
      canDuplicateClone, chanceToDuplicate, player, attackMultiplier);
  }

  public void CreateCloneOnDashStart()
  {
    if (createCloneOnDashStart)
    {
      CreateClone(player.transform, Vector3.zero );
    }
  }

  public void CreateCloneOnDashOver()
  {
    if (createCloneOnDashOver)
    {
      CreateClone(player.transform, Vector3.zero);
    }
  }

  public void CreateCloneOnCounterAttack(Transform _enemyTransform)
  {
    if (canCreateCloneOnCounterAttack)
    {
      StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDirection, 0)));
    }
  }

  private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
  {
    yield return new WaitForSeconds(0.4f);
    CreateClone(_transform,_offset);
  }
}
