using Photon.Pun;
using UnityEngine;

public class HideLocalHead : MonoBehaviour
{
    [SerializeField] private Transform localHead;
    private void Awake()
    {

    }
    private void Start()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            if (localHead != null)
            {
                localHead.gameObject.SetActive(false);
            }
        }
    }
}
