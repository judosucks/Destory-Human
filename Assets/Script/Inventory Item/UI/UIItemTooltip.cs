using UnityEngine;
using TMPro;
using UnityEditor;

public class UIItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private int defaultFontSize = 32;
    public void ShowTooltip(ItemDataEquipment item)
    {
        if (item == null) return;
        itemNameText.text = item.itemName;
        itemTypeText.text = item.equitmentType.ToString();
        itemDescription.text = item.GetDescription();

        if (itemNameText.text.Length > 15)
        {
            Debug.Log("too long");
            itemNameText.fontSize = itemNameText.fontSize * .7f;
        }
        else
        {
            itemNameText.fontSize = defaultFontSize;
        }
        
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
