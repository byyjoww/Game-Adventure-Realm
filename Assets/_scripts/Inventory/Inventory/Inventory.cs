using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Elysium.Attributes;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Item/Inventory")]
public class Inventory : ScriptableObject
{
    [Range(0, 100)] public int space = 20;
    [SerializeField] private List<InventoryStack> allInventoryItems = new List<InventoryStack>();
    public List<InventoryStack> AllInventoryItems => allInventoryItems;
    public event Action OnInventoryChanged;

    public int CheckItemAmount(IInventoryElement item)
    {
        if (item.IsStackable)
        {
            var singleStack = allInventoryItems.SingleOrDefault(x => x.Item == item);
            if (singleStack == null) { return 0; }
            else { return singleStack.amount; }
        }
        else
        {
            var allStacks = allInventoryItems.Where(x => x.Item == item);
            if (allStacks == null || allStacks.Count() < 1) { return 0; }
            else { return allStacks.Count(); }
        }        
    }

    public bool GainItem(IInventoryElement item, int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Invalid value for <amount>. Value must be a positive integer.");
        }

        if (item.IsStackable)
        {
            InventoryStack slot = allInventoryItems.SingleOrDefault(x => x.Item == item);
            if (slot == null)
            {
                allInventoryItems.Add(new InventoryStack(item, amount));

                if (AllInventoryItems.Count + 1 > space)
                {
                    Debug.LogError($"No space in inventory! | Space: {space} | ItemCount: {AllInventoryItems.Count}");
                    return false;
                }
            }
            else
            {
                slot.amount += amount;
            }
        }
        else
        {
            if (AllInventoryItems.Count + amount > space)
            {
                Debug.LogError($"No space in inventory! | Space: {space} | ItemCount: {AllInventoryItems.Count}");
                return false;
            }

            for (int i = 0; i < amount; i++)
            {
                allInventoryItems.Add(new InventoryStack(item, 1));
            }
        }

        OnInventoryChanged?.Invoke();
        Debug.Log($"Player gained x{amount} {item.ItemName}.");
        return true;
    }

    public bool LoseItem(IInventoryElement item, int amount)
    {
        if (CheckItemAmount(item) < amount)
        {
            Debug.Log($"Player has insufficient {item.ItemName}.");
            return false;
        }

        if (amount < 0)
        {
            throw new ArgumentException("Invalid value for <amount>. Value must be a positive integer.");
        }

        InventoryStack slot = allInventoryItems.First(x => x.Item == item);
        if (slot == null)
        {
            Debug.Log($"Player has insufficient {item.ItemName}.");
            return false;
        }
        else
        {
            slot.amount -= amount;
            if (slot.amount < 0)
            {
                Debug.LogError($"{item} amount just went below 0, after having {amount} units removed.");
                allInventoryItems.Remove(slot);
            }
            else if (slot.amount == 0)
            {
                allInventoryItems.Remove(slot);
            }
        }

        OnInventoryChanged?.Invoke();
        Debug.Log($"Player lost x{amount} {item.ItemName}.");
        return true;
    }
}

[System.Serializable]
public class InventoryStack
{
    [SerializeField, RequireInterface(typeof(IInventoryElement))] private ScriptableObject item;
    public IInventoryElement Item => item as IInventoryElement;
    public int amount;

    public InventoryStack(IInventoryElement item, int amount)
    {
        this.item = item as ScriptableObject;
        this.amount = amount;
    }
}