using Photon.Pun;
using Photon.Realtime;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class PhotonMgr : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button startBtn;
    public static PhotonMgr Instance;
    private void Awake()
    {
        PhotonNetwork.GameVersion = "1.0";
        Instance = this;
    }
    private void Start()
    {
        PhotonNetwork.NickName = Unity.Services.Authentication.AuthenticationService.Instance.PlayerId;
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