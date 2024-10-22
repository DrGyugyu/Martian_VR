using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action<Item> OnItemListChanged;
    private List<Item> itemList;
    public Inventory()
    {
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
    public List<Item> GetItemList()
    {
        return itemList;
    }
}