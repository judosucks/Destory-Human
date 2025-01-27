using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class UISkillTreeSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
   private UI ui;
   [SerializeField]private int skillExperience;
   [SerializeField]private string skillName;
   [SerializeField]private string skillDescription;
   [SerializeField] private Color lockedSkillColor;
   public bool unlocked;
   [SerializeField] private UISkillTreeSlot[] shouldBeLocked;
   [SerializeField] private UISkillTreeSlot[] shouldBeUnlocked;
   private Image skillImage;

   private void OnValidate()
   {
      gameObject.name = "UI Skill Tree Slot:"+skillName;
   }

   private void Awake()
   {
      GetComponent<Button>().onClick.AddListener(()=>UnlockSkillSlot());
   }

   private void Start()
   {
      ui = GetComponentInParent<UI>();
      skillImage = GetComponent<Image>();
      skillImage.color = lockedSkillColor;
      
   }

   public void UnlockSkillSlot()
   {
      if (PlayerManager.instance.HaveEnoughExperience(skillExperience) == false)
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
      ui.skillTreeTooltip.ShowSkillTreeTooltip(skillDescription, skillName);
      Vector2 mousePosition = Mouse.current.position.ReadValue();
      float xOffset = 0;
      float yOffset = 0;
      if (mousePosition.x > Screen.width)
      {
         xOffset = -150;
      }
      else
      {
         xOffset = 150;
      }

      if (mousePosition.y > Screen.height)
      {
         yOffset = -150;
      }
      else
      {
         yOffset = 150;
      }

      ui.skillTreeTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
   }
   

   public void OnPointerExit(PointerEventData eventData)
   {
      ui.skillTreeTooltip.HideSkillTreeTooltip();
   }
}
