using System;
using System.Threading.Tasks;
using Photon.Pun;
using TMPro;
using Unity.Cinemachine;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;


public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private GameObject playerVisual;
    private InventoryUI inventoryUI;
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    private XRBodyTransformer xRBodyTransformer;
    private CustomBodyPositionEvaluator customBodyPositionEvaluator;
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (playerVisual != null)
        {
            Vector3 rotation = playerVisual.transform.rotation.eulerAngles;
            rotation.y = camera.transform.rotation.eulerAngles.y;
            playerVisual.transform.rotation = Quaternion.Euler(rotation);
        }
        if (xRBodyTransformer != null && playerVisual != null)
        {
            UpdateTransform();
        }
    }
    private void AddToInventory(ItemWorld itemWorld)
    {
        GameMgr.Instance.GetInventoryOrigin().AddItem(itemWorld.GetItem());
        itemWorld.DestroySelf();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemWorld itemWorld))
        {
            AddToInventory(itemWorld);
        }
    }
    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "MarsScene")
        {
            PlayerVisualInit();
        }
    }
    private void PlayerVisualInit()
    {
        playerVisual = PhotonNetwork.Instantiate("PlayerVisual", Vector3.zero, Quaternion.identity);
        playerVisual?.GetComponent<PlayerVisual>().onPlayerVisualInit?.Invoke();
    }
    private void SetupBodyTransformer()
    {
        xRBodyTransformer = GetComponent<XRBodyTransformer>();
        customBodyPositionEvaluator = new CustomBodyPositionEvaluator(playerVisual.transform);
        xRBodyTransformer.bodyPositionEvaluator = customBodyPositionEvaluator;
    }
    private void UpdateTransform()
    {
        transform.position = playerVisual.transform.position;
        transform.rotation = playerVisual.transform.rotation;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
public class CustomBodyPositionEvaluator : IXRBodyPositionEvaluator
{
    private Transform visualTransform;

    public CustomBodyPositionEvaluator(Transform visual)
    {
        visualTransform = visual;
    }

    public Vector3 GetBodyGroundLocalPosition(float characterHeight, Transform origin)
    {
        if (visualTransform == null)
            return Vector3.zero;

        // Get the position of the PlayerVisual
        Vector3 visualPosition = visualTransform.position;

        // Convert to local space relative to the XR origin
        Vector3 localPosition = origin.InverseTransformPoint(visualPosition);

        // You might want to adjust the Y position based on your needs
        // localPosition.y = 0; // If you want to keep it grounded

        return localPosition;
    }

    public Vector3 GetBodyGroundLocalPosition(XROrigin xrOrigin)
    {
        throw new NotImplementedException();
    }
}