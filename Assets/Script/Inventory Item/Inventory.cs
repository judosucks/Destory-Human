using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using Yushan.Enums;


public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<ItemData> defaultItems;
    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;
    public List<InventoryItem> equipment;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;

    [Header("inventory ui")] [SerializeField]
    private Transform inventorySlotParent;

    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;
    private UIItemSlot[] inventoryItemSlot;
    private UIItemSlot[] stashItemSlot;
    private UIEquipmentSlot[] equipmentItemSlot;
    private UIStatSlot[] statSlot;

    [Header("Item cooldown")] 
    private float lastTimeUsedFlask;

    private float lastTimeUsedArmor;
    private float flaskCooldown;
    private float armorCooldown;
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
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UIItemSlot>();
        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();
        equipmentItemSlot = equipmentSlotParent.GetComponentsInChildren<UIEquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UIStatSlot>();
        DefaultItems();
    }

    private void DefaultItems()
    {
        for (int i = 0; i < defaultItems.Count; i++)
        {
            if(defaultItems[i] != null)
            AddItem(defaultItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;

        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemDataEquipment oldEquipment = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equitmentType == newEquipment.equitmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnequipItem(ItemDataEquipment _itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(_itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(_itemToRemove);
            _itemToRemove.RemoveModifiers();

        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentItemSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equitmentType == equipmentItemSlot[i].equitmentType)
                {
                    equipmentItemSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        for (int i = 0; i < statSlot.Length; i++)//update info of stat slot
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public bool CanAddItem()
    {
        if (inventory.Count >= inventoryItemSlot.Length)
        {
            Debug.Log("inventory full");
            return false;
        }
        return true;
    }
    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemnType.Equipment && CanAddItem())
        {
            AddItemToInventory(_item);
        }
        else if (_item.itemType == ItemnType.Material)
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

    public bool CanCraft(ItemDataEquipment _itemToCraft, List<InventoryItem> _requireMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        for (int i = 0; i < _requireMaterials.Count; i++)
        {
            if (!stashDictionary.TryGetValue(_requireMaterials[i].data, out InventoryItem stashValue) || stashValue.stackSize < _requireMaterials[i].stackSize)
            {
                Debug.Log("not enough materials");
                return false;
            }
            else
            {
                materialsToRemove.Add(stashValue);
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            if (materialsToRemove[i].data != null)
            {
                RemoveItem(materialsToRemove[i].data);
            }
        }

        AddItem(_itemToCraft);
        Debug.Log("here is your item" + _itemToCraft.name);
        return true;
    }

    public List<InventoryItem> GetEquipment() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemDataEquipment GetEquipmentByType(EquitmentType _type)
    {
        ItemDataEquipment equipedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equitmentType == _type)
            {
                equipedItem = item.Key;
            }
        }

        return equipedItem;
    }

    public void UseFlask()
    {
        ItemDataEquipment currentFlask = GetEquipmentByType(EquitmentType.Flask);
        if (currentFlask == null)
        {
            return;
        }

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;
        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.ItemEffect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log("cant use flask");
        }
    }

    public bool CanUseArmor()
    {
        ItemDataEquipment currentArmor = GetEquipmentByType(EquitmentType.Armor);
        if (Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }
        Debug.Log("cant use armor");
        return false;
    }

}
