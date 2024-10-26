using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
[Serializable]
public struct Item
{
    public ItemSO itemSO;
    public int amount;
    public ItemType GetItemType()
    {
        return itemSO.itemType;
    }
    public Sprite GetSprite()
    {
        return itemSO.itemSprite;
    }
    public GameObject GetItemObj()
    {
        return itemSO.itemObj;
    }
    public override string ToString()
    {
        StackTrace stackTrace = new StackTrace();
        StackFrame[] frames = stackTrace.GetFrames();

        if (frames.Any(frame => frame.GetMethod()?.DeclaringType?.Name == "GameMgr"))
        {
            return $"{itemSO.itemName} {amount}";
        }

        else return itemSO.itemName;
    }
}
public enum ItemType
{
    Burger,
    SolarPanel,
    Rock,
    Ore,
    PickAx
}