using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class UISkillTreeSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
   private SkillManager skillManager;
   private UI ui;
   [SerializeField]private int skillCost;
   [SerializeField]private string skillName;
   [SerializeField]private string skillDescription;
   [SerializeField] private Color lockedSkillColor;
   public bool unlocked;
   [SerializeField] private UISkillTreeSlot[] shouldBeLocked;
   [SerializeField] private UISkillTreeSlot[] shouldBeUnlocked;
   private Image skillImage;
   public static bool IsInitialized { get; private set; } = false; // Check if initialized

   private void OnValidate()
   {
      gameObject.name = "UI Skill Tree Slot:"+skillName;
   }

   private void Awake()
   {
      skillManager = SkillManager.instance;
      InitializeSkillTreeSlot();
      Debug.Log("awake from uiskilltreeslots");
      GetComponent<Button>().onClick.AddListener(()=>UnlockSkillSlot());
      
   }

   private void Start()
   {
      ui = GetComponentInParent<UI>();
      skillImage = GetComponent<Image>();
      skillImage.color = lockedSkillColor;
      
   }
   private void InitializeSkillTreeSlot()
   {
      // Perform any necessary initialization
      IsInitialized = true; // Mark as initialized
      Debug.Log("UISkillTreeSlot Initialized");
   }
   public void UnlockSkillSlot()
   {
      if (PlayerManager.instance.HaveEnoughExperience(skillCost) == false)
      {
         Debug.LogWarning("not enough experience");
         
         return;
      }
      Debug.Log("unlocking");
      for (int i = 0; i < shouldBeUnlocked.Length; i++)
      {
         if (shouldBeUnlocked[i].unlocked == false)
         {
            Debug.Log("can't unlock");
            
            return;
         }
      }

      for (int i = 0; i < shouldBeLocked.Length; i++)
      {
         if (shouldBeLocked[i].unlocked == true)
         {
            Debug.Log("can't lock");
            
            return;
         }
      }
      unlocked = true;
      skillImage.color = Color.white;
      
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      ui.skillTreeTooltip.ShowSkillTreeTooltip(skillDescription, skillName,skillCost);
      
   }
   

   public void OnPointerExit(PointerEventData eventData)
   {
      ui.skillTreeTooltip.HideSkillTreeTooltip();
   }
}
