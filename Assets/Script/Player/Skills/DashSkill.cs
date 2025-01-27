using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DashSkill : Skill
{
   
   [Header("dash")] 
   public bool dashUnlocked;
   [SerializeField] private UISkillTreeSlot dashUnlockedButton;
   [Header("clone on dash")] 
   public bool cloneOnDashUnlocked;
   [SerializeField] private UISkillTreeSlot cloneOnDashUnlockedButton;
   [Header("clone on dash arrival")] 
   public bool cloneOnDashArrivalUnlocked;
   [SerializeField] private UISkillTreeSlot cloneOnDashArrivalUnlockedButton;

   public override void UseSkill()
   {
      base.UseSkill();
      Debug.Log("created clone behind");
   }

   protected override void Start()
   {
      base.Start();
      dashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
      cloneOnDashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
      cloneOnDashArrivalUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDashArrival);
   }

   private void UnlockDash()
   {
      Debug.Log("attempt to unlocked dash");
      if (dashUnlockedButton.unlocked)
      {
        dashUnlocked = true;
        Debug.Log("unlocked dash"); 
      }
   }

   private void UnlockCloneOnDash()
   {
      if (cloneOnDashUnlockedButton.unlocked)
      {
        cloneOnDashUnlocked = true;
      }
   }
   private void UnlockCloneOnDashArrival()
   {
      if (cloneOnDashArrivalUnlockedButton.unlocked)
      {
        cloneOnDashArrivalUnlocked = true;
      }
   }

}
