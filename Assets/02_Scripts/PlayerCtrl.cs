using System;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private GameObject playerVisual;
    private InventoryUI inventoryUI;
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    [HideInInspector] public GameObject networkLeftHandTarget;
    [HideInInspector] public GameObject networkRightHandTarget;
    public static PlayerCtrl Instance;
    [SerializeField] private GameObject[] StartingObj;

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
        if (networkLeftHandTarget != null && networkRightHandTarget != null)
        {
            networkLeftHandTarget.transform.position = leftHandTarget.position;
            networkRightHandTarget.transform.position = rightHandTarget.position;
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
            foreach (GameObject gameObject in StartingObj)
            {
                gameObject.SetActive(false);
            }
            await Task.Delay(500);
            PlayerVisualInit();
            await Task.Delay(2000);
            GameMgr.Instance.StartDay(1);
        }
    }
    private void PlayerVisualInit()
    {
        playerVisual = PhotonNetwork.Instantiate("PlayerVisual", Vector3.zero, Quaternion.identity);
        networkLeftHandTarget = PhotonNetwork.Instantiate("LeftHandTarget", leftHandTarget.transform.position, Quaternion.identity);
        networkRightHandTarget = PhotonNetwork.Instantiate("RightHandTarget", rightHandTarget.transform.position, Quaternion.identity);
        playerVisual?.GetComponent<PlayerVisual>().onPlayerVisualInit?.Invoke();
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}