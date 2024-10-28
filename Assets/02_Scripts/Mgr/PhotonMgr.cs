using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonMgr : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button startBtn;
    public static PhotonMgr Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        PhotonNetwork.GameVersion = "1.0";
    }
    private void Start()
    {
        PhotonNetwork.NickName = Unity.Services.Authentication.AuthenticationService.Instance.PlayerName;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Joined");
        startBtn.transform.localScale = 2 * Vector3.one;
        PhotonNetwork.JoinRandomOrCreateRoom(null, 3);
    }
}