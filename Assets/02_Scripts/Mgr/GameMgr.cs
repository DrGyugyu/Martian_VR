using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameMgr : MonoBehaviour
{
    public static Camera camera;
    [SerializeField] UniversalRendererData pc_Renderer;
    private FullScreenPassRendererFeature fullScreenPassRendererFeature;
    private Vignette vignette;
    public static CharacterController playerCharacterCtrl;
    public static TMP_Text dailyMissionText;
    public static ColorAdjustments colorAdjustments;
    private Inventory inventory;
    public static InventoryUI inventoryUI;
    [SerializeField] private List<Item> day1Items;
    [SerializeField] private List<Item> day2Items;
    [SerializeField] private List<Item> day3Items;
    private List<Item>[] dailyItemListArr;
    public static GameMgr Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        colorAdjustments.colorFilter.value = Color.white;
    }
    private void Start()
    {
        playerCharacterCtrl.enabled = true;
        dailyItemListArr = new List<Item>[3] { day1Items, day2Items, day3Items };
        fullScreenPassRendererFeature = (FullScreenPassRendererFeature)pc_Renderer.rendererFeatures[0];
        StartDay(1);
    }
    private void StartDay(int day)
    {
        if (day > 3) return;
        ItemSpawner.Instance.SpawnDailyItems(day);
        dailyMissionText.GetComponent<TextEffect>().enabled = true;
        inventory = new Inventory(dailyItemListArr[day - 1], day);
        inventoryUI.SetInventory(inventory);
        inventory.OnDailyMissionComplete += DailyMissionComplete;

    }
    private async void DailyMissionComplete(int day)
    {
        inventory.OnDailyMissionComplete -= DailyMissionComplete;
        inventory = null;
        dailyMissionText.GetComponent<TextEffect>().enabled = false;
        ItemSpawner.Instance.DestroyAllItems();
        await Intermission();
        StartDay(day + 1);
    }
    private async Task Intermission()
    {
        Volume volume = camera.GetComponent<Volume>();
        if (volume.profile.TryGet(out Vignette vignette))
        {
            this.vignette = vignette;
        }
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustment))
        {
            colorAdjustments = colorAdjustment;
        }
        await Task.WhenAll(VignetteEffectAsync(vignette), ColorEffectAsync(colorAdjustments));
    }
    private Task VignetteEffectAsync(Vignette vignette)
    {
        var tcs = new TaskCompletionSource<bool>();
        StartCoroutine(VignetteEffectCoroutine(vignette, tcs));
        return tcs.Task;
    }

    private IEnumerator VignetteEffectCoroutine(Vignette vignette, TaskCompletionSource<bool> tcs)
    {
        float duration = 2.0f;
        int blinks = 2;

        for (int i = 0; i < blinks; i++)
        {
            float elapsed = 0f;
            while (elapsed < duration / (blinks * 2))
            {
                vignette.intensity.value = Mathf.Lerp(0, 1, elapsed / (duration / (blinks * 2)));
                elapsed += Time.deltaTime;
                yield return null;
            }
            elapsed = 0f;
            while (elapsed < duration / (blinks * 2))
            {
                vignette.intensity.value = Mathf.Lerp(1, 0, elapsed / (duration / (blinks * 2)));
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        tcs.SetResult(true);
    }

    private Task ColorEffectAsync(ColorAdjustments colorAdjustments)
    {
        var tcs = new TaskCompletionSource<bool>();
        StartCoroutine(ColorEffectCoroutine(colorAdjustments, tcs));
        return tcs.Task;
    }

    private IEnumerator ColorEffectCoroutine(ColorAdjustments colorAdjustments, TaskCompletionSource<bool> tcs)
    {
        yield return new WaitForSeconds(2);
        tcs.SetResult(true);
    }
    public Inventory GetInventoryOrigin()
    {
        return this.inventory;
    }
}