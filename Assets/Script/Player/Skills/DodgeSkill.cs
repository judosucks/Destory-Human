using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class DodgeSkill : Skill
{
    [SerializeField]private int evasionAmount;
    [Header("Dodge Skill")] [SerializeField]
    private UISkillTreeSlot unlockDodgeButton;

    public bool dodgeUnlocked;

    [SerializeField] private UISkillTreeSlot unlockDodgeMirageButton;
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(WaitForSkillTreeSlotInitialization());
        
    }

    protected override void CheckUnlocked()
    {
        UnlockDodge();
        unlockDodgeMirage();
    }


    private IEnumerator WaitForSkillTreeSlotInitialization()
    {
       while (!UISkillTreeSlot.IsInitialized)
       {
          yield return null; // Wait for one frame
       }
      unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
      unlockDodgeMirageButton.GetComponent<Button>().onClick.AddListener(unlockDodgeMirage);
      isSkillInitialized = true;
    }

private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatSlotUI();
            dodgeUnlocked = true;
        }
    }
    private void unlockDodgeMirage()
    {
        if (unlockDodgeMirageButton.unlocked)
        {
            dodgeMirageUnlocked = true;
        }
    }

    public void DodgeWithMirage()
    {
        if (dodgeMirageUnlocked)
        {
          Debug.Log("dodge with mirage");
          SkillManager.instance.cloneSkill.CreateClone(player.transform, new Vector3(2,0));
        }
    }
}
