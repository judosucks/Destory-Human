using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftSlot : UIItemSlot
{
    public void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemDataEquipment craftData = item.data as ItemDataEquipment;
        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
    }

}
