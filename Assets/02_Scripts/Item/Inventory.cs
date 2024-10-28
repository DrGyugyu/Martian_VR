using System;
using System.Collections.Generic;
using System.Linq;
public class Inventory
{
    public event Action<Item> OnItemListChanged;
    public event Action<int> OnDailyMissionComplete;
    private List<Item> itemList;
    private List<Item> missionItemList;
    private int day;
    public Inventory(List<Item> missionItemList, int day)
    {
        this.missionItemList = missionItemList;
        this.day = day;
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
        if (missionItemList.All(missionItem => itemList.Any(item => item.GetItemType() == missionItem.GetItemType() && item.amount >= missionItem.amount)))
        {
            OnDailyMissionComplete?.Invoke(day);
        }
    }
    public List<Item> GetItemList()
    {
        return itemList;
    }
}