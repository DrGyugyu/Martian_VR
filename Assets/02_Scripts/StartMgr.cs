using System.Collections;
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
    private Volume volume;
    private ColorAdjustments colorAdjustments;
    private bool isRed;
    private void OnEnable()
    {
        startBtn.onClick.AddListener(() =>
        {
            cameraRb.useGravity = true;
            particalSystem.gameObject.SetActive(true);
            startBtn.gameObject.transform.localScale = Vector3.zero;
        });
    }
    private void OnDisable()
    {
        startBtn.onClick.RemoveListener(() =>
        {
            cameraRb.useGravity = true;
            particalSystem.gameObject.SetActive(true);
            startBtn.gameObject.transform.localScale = Vector3.zero;
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
    private void OnTriggerEnter(Collider other)
    {
        isRed = false;
        StopCoroutine(FlashScreen());
        StartCanvas.gameObject.SetActive(false);
        playerVisual.gameObject.SetActive(true);
        player.transform.rotation = Quaternion.identity;
        player.transform.position = Vector3.zero;
        GameMgr.playerCharacterCtrl = characterController;
        Destroy(cameraRb);
        SceneManager.LoadScene("MarsScene");
    }
}