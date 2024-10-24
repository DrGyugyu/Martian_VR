using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;
    [SerializeField] private List<Item> day1SpawnItemList;
    [SerializeField] private List<Item> day2SpawnItemList;
    [SerializeField] private List<Item> day3SpawnItemList;
    private List<Item>[] dailySpawnItemListArr;
    [SerializeField] private Transform[] spawnLocTr;
    private bool dayStarted;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        dailySpawnItemListArr = new List<Item>[3] { day1SpawnItemList, day2SpawnItemList, day3SpawnItemList };
    }
    public void SpawnDailyItems(int day)
    {
        ShuffleTrArray(spawnLocTr);
        for (int i = 0; i < dailySpawnItemListArr[day - 1].Count; i++)
        {
            ItemWorld.SpawnItemWorld(spawnLocTr[i].position, dailySpawnItemListArr[day - 1][i]);
        }
    }
    private void ShuffleTrArray(Transform[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = Random.Range(i, array.Length);
            Transform temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
    public void DestroyAllItems()
    {
        ItemWorld[] itemGameObj = FindObjectsOfType<ItemWorld>();
        foreach (var item in itemGameObj)
        {
            Destroy(item.gameObject);
        }
    }
}