using System;
using System.Threading.Tasks;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private readonly int hashMovement = Animator.StringToHash("moveSpeed");
    [SerializeField] private InputActionProperty XRMoveAction;
    [SerializeField] private Transform localHead;
    [SerializeField] private ChainIKConstraint leftHandAim;
    [SerializeField] private ChainIKConstraint rightHandAim;
    private PlayerCtrl player;
    private Transform leftHandTarget;
    private Transform rightHandTarget;
    public Action onPlayerVisualInit;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Material[] playerMaterials;
    private int materialIndex;
    [SerializeField] private TMP_Text playerText;
    public TMP_Text dailyMissionText;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private RectTransform canvas;
    private void OnEnable()
    {
        onPlayerVisualInit += SetPlayerVisual;
        XRMoveAction.action.performed += (ctx) =>
        {
            Vector2 moveInput = ctx.ReadValue<Vector2>();
            animator.SetFloat(hashMovement, moveInput.magnitude * Mathf.Sign(moveInput.y));
        };
        XRMoveAction.action.canceled += (ctx) =>
        {
            animator.SetFloat(hashMovement, 0);
        };
    }
    private void OnDisable()
    {
        onPlayerVisualInit -= SetPlayerVisual;
        XRMoveAction.action.performed -= (ctx) =>
        {
            animator.SetFloat(hashMovement, ctx.ReadValue<Vector2>().magnitude);
        };
        XRMoveAction.action.canceled -= (ctx) =>
        {
            animator.SetFloat(hashMovement, 0);
        };
    }
    private async void Start()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            if (localHead != null)
            {
                localHead.localScale = Vector3.zero;
            }
        }
        GameMgr.playerVisual = this;
        GameMgr.inventoryUI = inventoryUI;



        materialIndex = UnityEngine.Random.Range(0, playerMaterials.Length);
        skinnedMeshRenderer.material = playerMaterials[materialIndex];
        photonView.RPC("SetMaterial", RpcTarget.AllBuffered, materialIndex);
        photonView.RPC("SetPlayerName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        photonView.RPC("HidePlayerCanvas", RpcTarget.AllBuffered);
    }
    private void Update()
    {
        transform.position = new Vector3(
            player.transform.position.x,
            transform.position.y,
            player.transform.position.z
        );
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            Camera.main.transform.eulerAngles.y,
            transform.eulerAngles.z
        );
    }
    private void LateUpdate()
    {
        playerText.transform.LookAt(Camera.main.transform.position);
    }

    public void SetPlayerVisual()
    {
        player = FindAnyObjectByType<PlayerCtrl>();


        leftHandTarget = player.GetComponent<PlayerCtrl>().leftHandTarget;
        rightHandTarget = player.GetComponent<PlayerCtrl>().rightHandTarget;
        leftHandAim.data.target = leftHandTarget;
        rightHandAim.data.target = rightHandTarget;
        RigBuilder rigBuilder = GetComponent<RigBuilder>();
        rigBuilder.enabled = false;
        rigBuilder.Build();
        rigBuilder.enabled = true;
    }
    public void DailyMissionTxt(string txt)
    {
        Debug.Log(txt);
        dailyMissionText.text = txt;
        TextEffect textEffect = dailyMissionText.GetComponent<TextEffect>();
        textEffect.SetNewText(txt);  // Set the new text before enabling
        textEffect.enabled = true;
    }
    [PunRPC]
    private void SetMaterial(int index)
    {
        materialIndex = index;
        skinnedMeshRenderer.material = playerMaterials[materialIndex];
    }
    [PunRPC]
    private void SetPlayerName(string playername)
    {
        playerText.text = playername;
    }
    [PunRPC]
    private void HidePlayerCanvas()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            if (canvas != null)
            {
                canvas.localScale = Vector3.zero;
            }
        }
    }
}
