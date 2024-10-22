using System;
using System.Collections.Generic;
using System.Linq;
public class Inventory
{
    public event Action<Item> OnItemListChanged;
    public event Action OnDailyMissionComplete;
    private List<Item> itemList;
    private List<Item> missionItemList;
    public Inventory(List<Item> missionItemList)
    {
        this.missionItemList = missionItemList;
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
        if (itemList.All(missionItem => missionItemList.Any(item => item.GetItemType() == missionItem.GetItemType() && item.amount >= missionItem.amount)))
        {
            OnDailyMissionComplete?.Invoke();
        }
    }
    public List<Item> GetItemList()
    {
        return itemList;
    }
}