using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemSlotTemplate;
    public ItemSO solarPanelSO;
    public ItemSO pickAxSO;
    public ItemSO MRESO;
    public ItemSO rockSO;
    public ItemSO oreSO;
    [SerializeField] private PlayerCtrl playerCtrl;

    [SerializeField] private Transform itemSlotContainer;
    public static InventoryUI Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += OnItemListChanged;
    }

    private void OnItemListChanged(Item item)
    {
        RefreshInventoryItems(item);
    }
    private void RefreshInventoryItems(Item newItem)
    {
        foreach (Transform itemSlot in itemSlotContainer)
        {
            if (itemSlot == itemSlotTemplate) continue;
            Destroy(itemSlot.gameObject);
        }
        foreach (Item item in inventory.GetItemList())
        {
            GameObject newItemSlot = Instantiate(itemSlotTemplate, itemSlotContainer);
            newItemSlot.transform.Find("IconImg").GetComponent<Image>().sprite = item.GetSprite();
            newItemSlot.GetComponentInChildren<TextMeshProUGUI>().text = item.amount.ToString();
            newItemSlot.GetComponent<ButtonUI>().ClickFunc = () =>
            {
                //Use Item
                inventory.UseItem(item);
            };
            newItemSlot.GetComponent<ButtonUI>().MouseRightClickFunc = () =>
            {
                //Drop Item
                ItemWorld.DropItem(playerCtrl.transform.position, item);
                inventory.RemoveItem(item);
            };

        }
    }
}