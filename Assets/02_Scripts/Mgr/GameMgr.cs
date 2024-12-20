using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Photon.Pun;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public static Camera camera;
    [SerializeField] UniversalRendererData pc_Renderer;
    private FullScreenPassRendererFeature fullScreenPassRendererFeature;
    private Vignette vignette;
    public static CharacterController playerCharacterCtrl;
    public static string dailyMissionText;
    public static ColorAdjustments colorAdjustments;
    private Inventory inventory;
    public static InventoryUI inventoryUI;
    public static GameObject player;
    [SerializeField] private List<Item> day1Items;
    [SerializeField] private List<Item> day2Items;
    [SerializeField] private List<Item> day3Items;
    private List<Item>[] dailyItemListArr;
    private Button leaveBtn;
    public static Canvas gameClearCanvas;
    public static Button gameClearBtn;
    public static GameMgr Instance;
    public static PlayerVisual playerVisual;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else
        {
            Instance = this;
            //DontDestroyOnLoad(Instance);
        }
        colorAdjustments.colorFilter.value = Color.white;
        playerCharacterCtrl.enabled = true;
    }
    private void Start()
    {
        dailyItemListArr = new List<Item>[3] { day1Items, day2Items, day3Items };
        fullScreenPassRendererFeature = (FullScreenPassRendererFeature)pc_Renderer.rendererFeatures[0];
    }
    public void StartDay(int day)
    {
        if (day > 3)
        {
            GameClear();
            return;
        }
        playerCharacterCtrl.enabled = true;
        ItemSpawner.Instance.SpawnDailyItems(day);
        string dailyItems = string.Join(", ", dailyItemListArr[day - 1].Select(item => item.ToString()));
        string dailyMissionText = $"Day {day} Task\nCollect: {dailyItems}";
        playerVisual.DailyMissionTxt(dailyMissionText);
        inventory = new Inventory(dailyItemListArr[day - 1], day);
        inventoryUI.SetInventory(inventory);
        inventory.OnDailyMissionComplete += DailyMissionComplete;
    }
    private void GameClear()
    {
        gameClearCanvas.gameObject.SetActive(true);
        Destroy(playerVisual);
        gameClearBtn.onClick.AddListener(ReturnToStart);
    }
    public void ReturnToStart()
    {
        PhotonNetwork.Destroy(player);
        AuthenticationService.Instance.SignOut();
        AuthenticationService.Instance.ClearSessionToken();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("StartScene");
    }
    private async void DailyMissionComplete(int day)
    {
        playerCharacterCtrl.enabled = false;
        inventory.OnDailyMissionComplete -= DailyMissionComplete;
        inventory = null;
        playerVisual.dailyMissionText.GetComponent<TextEffect>().enabled = false;
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
        float duration = 2.0f;
        float elapsed = 0f;
        while (elapsed < duration / 2)
        {
            colorAdjustments.postExposure.value = Mathf.Lerp(0, -10, elapsed / (duration / 2)); // Adjust range as necessary
            elapsed += Time.deltaTime;
            yield return null;
        }
        elapsed = 0f;

        while (elapsed < duration / 2)
        {
            colorAdjustments.postExposure.value = Mathf.Lerp(-10, 0, elapsed / (duration / 2)); // Adjust range as necessary
            elapsed += Time.deltaTime;
            yield return null;
        }

        tcs.SetResult(true);
    }

    public Inventory GetInventoryOrigin()
    {
        return this.inventory;
    }
}