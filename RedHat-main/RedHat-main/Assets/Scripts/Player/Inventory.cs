using System;
using UnityEngine;

public enum ItemID
{
    Stick = 0,
    Stone = 1,
    Plantain = 2,
    Apple = 3
}

[System.Serializable]
public class InventorySlot
{
    public ItemID itemId;
    public InteractItem itemPrefab;
    public int maxStack;
    public int currentAmount;

    public bool IsFull => currentAmount >= maxStack;
    public bool IsEmpty => currentAmount <= 0;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlot[] slots;
    public event Action OnInventoryChanged;
    private void Start()
    {
        slots = new InventorySlot[]
        {
            new InventorySlot { itemId = ItemID.Stick, maxStack = 20 },
            new InventorySlot { itemId = ItemID.Stone, maxStack = 10 },
            new InventorySlot { itemId = ItemID.Plantain, maxStack = 6 },
            new InventorySlot { itemId = ItemID.Apple, maxStack = 6 }
        };
    }

    public bool AddItem(ItemID itemId, InteractItem itemPrefab)
    {
        InventorySlot slot = GetSlot(itemId);
        if (slot == null) return false;
        if (slot.IsFull) return false;

        if (slot.itemPrefab == null)
            slot.itemPrefab = itemPrefab;

        slot.currentAmount++;
        OnInventoryChanged?.Invoke();
        return true;

    }

    public bool RemoveItem(ItemID itemId, int amount = 1)
    {
        InventorySlot slot = GetSlot(itemId);
        if (slot == null || slot.currentAmount < amount)
            return false;

        slot.currentAmount -= amount;
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool HasItem(ItemID itemId, int amount = 1)
    {
        InventorySlot slot = GetSlot(itemId);
        return slot != null && slot.currentAmount >= amount;
    }


    public int GetItemCount(ItemID itemType)
    {
        // Реализуйте получение количества предметов
        // Пример:
        foreach (var slot in slots)
        {
            if (slot.itemId == itemType)
                return slot.currentAmount;
        }
        return 0;
    }
    public void UseItem(ItemID itemId)
    {
        InventorySlot slot = GetSlot(itemId);
        if (slot == null || slot.IsEmpty || slot.itemPrefab == null)
            return;

        var player = GetComponent<PlayerInput>();
        var newItem = Instantiate(slot.itemPrefab);
        newItem.Use(player);
        RemoveItem(itemId);
        Destroy(newItem, 5f);
    }

    private InventorySlot GetSlot(ItemID itemId)
    {
        foreach (var slot in slots)
            if (slot.itemId == itemId)
                return slot;
        return null;
    }
}