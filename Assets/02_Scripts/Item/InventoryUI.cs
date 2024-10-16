using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemSlotTemplate;
    [SerializeField] private Transform itemSlotContainer;
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        RefreshInventoryItems();
    }
    private void RefreshInventoryItems()
    {
        foreach (Item item in inventory.GetItemList())
        {
            itemSlotTemplate.GetComponent<Image>().sprite = item.GetSprite();
            Instantiate(itemSlotTemplate, itemSlotContainer);
        }
    }
}