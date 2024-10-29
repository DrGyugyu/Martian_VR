using System;
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
    [SerializeField] private Button startBtn;
    [SerializeField] private float flashSpeed = 1.0f;
    [SerializeField] private GameObject particalSystem;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] StartingObj;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera camera;
    [SerializeField] private Canvas gameClearCanvas;
    [SerializeField] private Button gameClearBtn;
    private Volume volume;
    private ColorAdjustments colorAdjustments;
    private bool isRed;
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AnonymousSignInAsync();
    }
    private void OnEnable()
    {
        startBtn.onClick.AddListener(async () =>
        {
            CrashLanding();
        });
    }
    private void OnDisable()
    {
        startBtn.onClick.RemoveListener(async () =>
        {
            CrashLanding();
        });
    }
    void Start()
    {
        Volume volume = camera.GetComponent<Volume>();
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            this.colorAdjustments = colorAdjustments;
        }
        StartCoroutine(FlashScreen());

        GameMgr.playerCharacterCtrl = characterController;

        GameMgr.camera = camera;
        GameMgr.colorAdjustments = colorAdjustments;
        GameMgr.player = player;
        GameMgr.gameClearCanvas = gameClearCanvas;
        GameMgr.gameClearBtn = gameClearBtn;
    }
    private void CrashLanding()
    {
        particalSystem.gameObject.SetActive(true);
        startBtn.gameObject.transform.localScale = Vector3.zero;
        StartCoroutine(MoveToPlayer());
    }

    private IEnumerator MoveToPlayer()
    {
        float duration = 1.0f; // Duration of the movement
        float elapsedTime = 0.0f;

        Vector3 startPosition = particalSystem.transform.position;
        Vector3 targetPosition = player.transform.position;

        while (elapsedTime < duration)
        {
            // Lerp the particle system's position towards the player's position
            this.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the particle system reaches the player's position
        particalSystem.transform.position = targetPosition;

        // Deactivate the particle system after it has reached the player
        particalSystem.gameObject.SetActive(false);
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
        StopCoroutine(FlashScreen());
        foreach (GameObject gameObject in StartingObj)
        {
            gameObject.SetActive(false);
        }

        player.transform.rotation = Quaternion.identity;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("MarsScene");
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}