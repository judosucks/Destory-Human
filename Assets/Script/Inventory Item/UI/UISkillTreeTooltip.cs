using System;
using TMPro;
using UnityEngine;

public class UISkillTreeTooltip : MonoBehaviour
{
   public RectTransform rect;
   [SerializeField]private TextMeshProUGUI skillNameText;
   [SerializeField] private TextMeshProUGUI skillDescriptionText;

   private void Start()
   {
      
      if (rect == null)
      {
         rect = GetComponent<RectTransform>();
      }
      
   }

   public void ShowSkillTreeTooltip(string _skillDescription, string _skillName)
   {
      Debug.Log("show enter");
      skillNameText.text = _skillName;
      skillDescriptionText.text = _skillDescription;
      gameObject.SetActive(true);
   }
   public void HideSkillTreeTooltip()=>gameObject.SetActive(false);

}
