using Photon.Pun;
using Photon.Realtime;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Composites;

public class PhotonMgr : MonoBehaviourPunCallbacks
{
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
        PhotonNetwork.JoinRandomOrCreateRoom(null, 3);
    }
}