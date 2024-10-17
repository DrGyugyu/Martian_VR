using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemSlotTemplate;
    public ItemSO solarPanelSO;

    [SerializeField] private Transform itemSlotContainer;
    public static InventoryUI Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        RefreshInventoryItems();
    }
    private void RefreshInventoryItems()
    {
        foreach (Item item in inventory.GetItemList())
        {
            GameObject newItemSlot = Instantiate(itemSlotTemplate, itemSlotContainer);
            newItemSlot.transform.Find("IconImg").GetComponent<Image>().sprite = item.GetSprite();
            newItemSlot.GetComponentInChildren<TextMeshProUGUI>().text = item.amount.ToString();
        }
    }
}