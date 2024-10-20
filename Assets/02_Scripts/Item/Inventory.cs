using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action<Item> OnItemListChanged;
    public event Action<Item> OnItemUsed;
    private List<Item> itemList;
    public Inventory(Action<Item> OnItemUsed)
    {
        this.OnItemUsed = OnItemUsed;
        itemList = new List<Item>();
    }
    public void AddItem(Item item)
    {
        int index = itemList.FindIndex(inventoryItem => inventoryItem.GetItemType() == item.GetItemType());

        if (index >= 0)
        {
            Item inventoryItem = itemList[index];
            inventoryItem.amount += item.amount;
            itemList[index] = inventoryItem;
        }
        else itemList.Add(item);
        OnItemListChanged?.Invoke(item);
    }
    public void UseItem(Item item)
    {
        OnItemUsed(item);
    }
    public void RemoveItem(Item item)
    {
        int index = itemList.FindIndex(inventoryItem => inventoryItem.GetItemType() == item.GetItemType());

        if (index >= 0)
        {
            Item inventoryItem = itemList[index];
            inventoryItem.amount -= item.amount;
            if (inventoryItem.amount <= 0)
            {
                itemList.RemoveAt(index);
            }
            else
            {
                itemList[index] = inventoryItem;
            }

            OnItemListChanged?.Invoke(inventoryItem);
        }
        else
        {
            Debug.LogWarning("Item to remove not found in inventory.");
        }
    }
    public List<Item> GetItemList()
    {
        return itemList;
    }
}