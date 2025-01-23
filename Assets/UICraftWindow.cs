using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UICraftWindow : MonoBehaviour
{
  [SerializeField]private TextMeshProUGUI itemName;
  [SerializeField]private TextMeshProUGUI itemDescription;
  [SerializeField]private Image itemIcon;
  [SerializeField] private Image[] materialImages;

  public void SetupCraftWindow(ItemDataEquipment _data)
  {
    for (int i = 0; i < materialImages.Length; i++)
    {
      materialImages[i].color = Color.clear;
      materialImages[i].GetComponentInChildren<TextMeshProUGUI>();
    }

    for (int i = 0; i < _data.craftingMaterials.Count; i++)
    {
      if (_data.craftingMaterials.Count > materialImages.Length)
      {
        Debug.Log("Not enough slot");
      }

      materialImages[i].sprite = _data.craftingMaterials[i].data.icon;
      materialImages[i].color = Color.white;
      TextMeshProUGUI materialSlotText = materialImages[i].GetComponentInChildren<TextMeshProUGUI>();
      
      materialSlotText.text= _data.craftingMaterials[i].stackSize.ToString();
      materialSlotText.color = Color.white;
    }
     itemIcon.sprite = _data.icon;
     itemName.text = _data.itemName;
     itemDescription.text = _data.GetDescription();
  }
  
}
