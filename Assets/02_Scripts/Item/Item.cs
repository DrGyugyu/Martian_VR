using UnityEngine;

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
        return itemSO.itemName;
    }

}
public enum ItemType
{
    MRE,
    SolarPanel,
    Rock,
    Ore
}