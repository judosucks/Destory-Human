using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Yushan.Enums;
using UnityEngine.InputSystem;
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

    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }
    public virtual void OnPointerDown(PointerEventData _eventData)
    {
        if(item == null || item.data == null) return;
        if (Keyboard.current.leftCtrlKey.isPressed)
        {
            Debug.Log("drop"+" "+item.data.itemName);
            Inventory.instance.RemoveItem(item.data);
            return;
        }
        if (item.data.itemType == ItemnType.Equipment)
        {
            Debug.Log("equip"+" "+item.data.itemName);
            Inventory.instance.EquipItem(item.data);
        }
    }
}
