using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using Yushan.Enums;


public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;
    public List<InventoryItem> equipment;
    public Dictionary<ItemDataEquipment,InventoryItem> equipmentDictionary;
    
    [Header("inventory ui")]
    [SerializeField]private Transform InventorySlotParent;
    [SerializeField]private Transform StashSlotParent;
    private UIItemSlot[] inventoryItemSlot;
    private UIItemSlot[] stashItemSlot;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        inventoryItemSlot = InventorySlotParent.GetComponentsInChildren<UIItemSlot>();
        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        stashItemSlot = StashSlotParent.GetComponentsInChildren<UIItemSlot>();
        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();
    }

    public void EquipItem(ItemData _item)
    {
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;
        
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemDataEquipment itemToRemove = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equitmentType == newEquipment.equitmentType)
            {
                itemToRemove = item.Key;
            }
        }

        if (itemToRemove != null)
        {
            UnequipItem(itemToRemove);
        }
        equipment.Add(newItem);
        equipmentDictionary.Add( newEquipment, newItem);
        RemoveItem(_item);
    }

    private void UnequipItem(ItemDataEquipment _itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(_itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(_itemToRemove);
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
    }
    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemnType.Equipment)
        {
          AddItemToInventory(_item);
        }else if (_item.itemType == ItemnType.Material)
        {
            AddItemToStash(_item);
        }
        UpdateSlotUI();
    }

    private void AddItemToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem vlaue))
        {
            vlaue.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddItemToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem vlaue))
        {
            vlaue.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem vlaue))
        {
            if (vlaue.stackSize <= 1)
            {
                inventory.Remove(vlaue);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                vlaue.RemoveStack();
            }
            
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }
        UpdateSlotUI();
    }

    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame && inventory.Count > 0)
        {
            ItemData newItem = inventory[inventory.Count - 1].data;
            RemoveItem(newItem);
        }
    }
}
