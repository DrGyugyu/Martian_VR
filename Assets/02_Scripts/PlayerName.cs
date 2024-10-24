using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TMP_Text>().text = PhotonNetwork.NickName;
    }
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}