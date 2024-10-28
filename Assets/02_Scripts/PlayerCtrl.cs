using System;
using System.Threading.Tasks;
using Photon.Pun;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private GameObject playerVisual;
    private InventoryUI inventoryUI;
    public Transform leftHandTarget;
    public Transform rightHandTarget;

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

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}