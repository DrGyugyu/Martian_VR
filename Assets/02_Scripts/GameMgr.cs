using System;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private FullScreenPassRendererFeature fullScreenPassRendererFeature;
    [SerializeField] private Canvas canvas;
    private Inventory inventory;
    [SerializeField] private InventoryUI inventoryUI;
    public List<Item> day1Items;
    public List<Item> day2Items;
    public List<Item> day3Items;
    public static GameMgr Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
    private void Start()
    {
        inventoryUI.SetInventory(inventory);
        fullScreenPassRendererFeature = camera.GetComponent<Renderer>().GetComponent<FullScreenPassRendererFeature>();
    }
    private void StartDay1()
    {
        inventory = new Inventory(day1Items);
        inventory.OnDailyMissionComplete += DailyMissionComplete;
        ItemWorld.SpawnItemWorld(new Vector3(0, 0, 5), new Item { itemSO = inventoryUI.solarPanelSO, amount = 4 });
        ItemWorld.SpawnItemWorld(new Vector3(0, 0, 8), new Item { itemSO = inventoryUI.solarPanelSO, amount = 1 });
        ItemWorld.SpawnItemWorld(Vector3.one * 2, new Item { itemSO = inventoryUI.pickAxSO, amount = 1 });
    }

    private void DailyMissionComplete()
    {
        inventory.OnDailyMissionComplete -= DailyMissionComplete;
        inventory = null;
    }

    private void StartDay2()
    {

    }
    private void StartDay3()
    {

    }
    public Inventory GetInventoryOrigin()
    {
        return this.inventory;
    }
}
