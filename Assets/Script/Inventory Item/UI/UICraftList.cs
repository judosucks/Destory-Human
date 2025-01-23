using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UICraftList : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;
    [SerializeField] private List<ItemDataEquipment> craftEquipments;
    [SerializeField] private List<UICraftSlot> craftSlots;

    private void Start()
    {
        AssignCraftSlots();
    }

    private void AssignCraftSlots()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            craftSlots.Add(craftSlotParent.GetChild(i).GetComponent<UICraftSlot>());
        }
    }

    public void SetupCraftList()
    {
        for(int i = 0; i < craftSlots.Count; i++)
        {
           Destroy(craftSlots[i].gameObject);
        }
        craftSlots = new List<UICraftSlot>();
        for (int i = 0; i < craftEquipments.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UICraftSlot>().SetupCraftSlot(craftEquipments[i]);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }
}
