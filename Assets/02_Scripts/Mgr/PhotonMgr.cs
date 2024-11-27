using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PhotonMgr : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button startBtn;
    public static PhotonMgr Instance;
    public static RoomOptions roomOptions = new RoomOptions
    {
        MaxPlayers = 3,
        IsVisible = true,
        IsOpen = true
    };
    public static string ROOM_NAME = "GameRoom";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");


        startBtn.transform.localScale = 2 * Vector3.one;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"Players in room: {PhotonNetwork.CurrentRoom.PlayerCount}");
        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            Debug.Log($"Player in room: {player.NickName}");
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"New player joined: {newPlayer.NickName}");
        Debug.Log($"Total players now: {PhotonNetwork.CurrentRoom.PlayerCount}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Failed to join room: {message}");
    }
}