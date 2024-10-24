using System.Collections;
using System.Threading.Tasks;
using Photon.Pun;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMgr : MonoBehaviour
{
    [SerializeField] private Rigidbody cameraRb;
    [SerializeField] private Button startBtn;
    [SerializeField] private float flashSpeed = 1.0f;
    [SerializeField] private GameObject particalSystem;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private GameObject StartCanvas;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera camera;
    [SerializeField] private TMP_Text dailyMissionText;
    [SerializeField] private InventoryUI inventoryUI;
    private Volume volume;
    private ColorAdjustments colorAdjustments;
    private bool isRed;
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
    }
    private void OnEnable()
    {
        startBtn.onClick.AddListener(async () =>
        {
            cameraRb.useGravity = true;
            particalSystem.gameObject.SetActive(true);
            startBtn.gameObject.transform.localScale = Vector3.zero;
            await AnonymousSignInAsync();
        });
    }
    private void OnDisable()
    {
        startBtn.onClick.RemoveListener(async () =>
        {
            cameraRb.useGravity = true;
            particalSystem.gameObject.SetActive(true);
            startBtn.gameObject.transform.localScale = Vector3.zero;
            await AnonymousSignInAsync();
        });
    }
    void Start()
    {
        cameraRb.useGravity = false;
        Volume volume = camera.GetComponent<Volume>();
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            this.colorAdjustments = colorAdjustments;
        }
        StartCoroutine(FlashScreen());
        GameMgr.playerCharacterCtrl = characterController;
        GameMgr.dailyMissionText = dailyMissionText;
        GameMgr.inventoryUI = inventoryUI;
    }
    private IEnumerator FlashScreen()
    {
        while (true)
        {
            isRed = !isRed;
            colorAdjustments.colorFilter.value = isRed ? Color.red : Color.white;
            yield return new WaitForSeconds(flashSpeed / 2.0f);
        }
    }
    private async Task AnonymousSignInAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (AuthenticationException e)
        {
            Debug.Log(e.Message);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        isRed = false;
        colorAdjustments.colorFilter.value = Color.white;
        StopCoroutine(FlashScreen());
        StartCanvas.gameObject.SetActive(false);
        playerVisual.gameObject.SetActive(true);
        player.transform.rotation = Quaternion.identity;
        player.transform.position = Vector3.zero;
        playerVisual.transform.rotation = Quaternion.identity;

        Destroy(cameraRb);
        SceneManager.LoadScene("MarsScene");
    }
}