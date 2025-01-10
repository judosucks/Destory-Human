using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Yushan.Enums;

public class UIItemSlot : MonoBehaviour , IPointerDownHandler
{
    [SerializeField]private Image itemImage;
    [SerializeField]private TextMeshProUGUI itemText;
    
    public InventoryItem item;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;
        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite =  item.data.icon;
            if(item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData _eventData)
    {
        if (item.data.itemType == ItemnType.Equipment)
        {
            Debug.Log("equip"+" "+item.data.itemName);
            Inventory.instance.EquipItem(item.data);
        }
    }
}
