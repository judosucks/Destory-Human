using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftSlot : UIItemSlot
{
    protected override void Start()
    {
        base.Start();
    }
    public void SetupCraftSlot(ItemDataEquipment _data)
    {
        if (_data == null) return;
        item.data = _data;
        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;
        
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemDataEquipment craftData = item.data as ItemDataEquipment;
        if (craftData != null)
        {
            Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
        }
    }

}
