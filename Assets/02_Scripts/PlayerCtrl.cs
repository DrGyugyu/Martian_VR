using System;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Animator animator;
    private readonly int hashMovement = Animator.StringToHash("moveSpeed");
    [SerializeField] private Transform playerVisual;
    [SerializeField] private Transform grabTranform;
    [SerializeField] XRRayInteractor rightHandInteractor;
    [SerializeField] private InputActionProperty XRMoveAction;
    [SerializeField] private InputActionProperty XRGrabAction;
    public Inventory inventory;
    [SerializeField] private InventoryUI inventoryUI;
    private void OnEnable()
    {
        XRMoveAction.action.performed += (ctx) =>
        {
            Vector2 moveInput = ctx.ReadValue<Vector2>();
            animator.SetFloat(hashMovement, moveInput.magnitude * Mathf.Sign(moveInput.y));
        };
        XRMoveAction.action.canceled += (ctx) =>
        {
            animator.SetFloat(hashMovement, 0);
        };
        XRGrabAction.action.performed += async (ctx) =>
        {
            await OnGrab(ctx);
        };
        XRGrabAction.action.canceled += (ctx) =>
        {
            OnRelease(ctx);
        };
    }
    private void OnDisable()
    {
        XRMoveAction.action.performed -= (ctx) =>
        {
            //Debug.Log(ctx.ReadValue<Vector2>());
            animator.SetFloat(hashMovement, ctx.ReadValue<Vector2>().magnitude);
        };
        XRMoveAction.action.canceled -= (ctx) =>
        {
            animator.SetFloat(hashMovement, 0);
        };
        XRGrabAction.action.performed -= (ctx) =>
        {
            OnGrab(ctx);
        };
        XRGrabAction.action.canceled -= (ctx) =>
        {
            OnRelease(ctx);
        };
    }
    private async Task OnGrab(InputAction.CallbackContext ctx)
    {
        try
        {
            await Task.Delay(10);
            XRGrabInteractable grabbedObj = rightHandInteractor.interactablesSelected[0] as XRGrabInteractable;
            grabbedObj.GetComponent<XRGrabInteractable>().enabled = false;
            grabbedObj.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObj.transform.parent = grabTranform;
            grabbedObj.transform.position = grabTranform.position;
            grabbedObj.transform.rotation = grabTranform.rotation;
            grabbedObj.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
        //grabbedObject?.gameObject.transform.
    }
    private void OnRelease(InputAction.CallbackContext ctx)
    {

    }
    private void Start()
    {
        inventory = new Inventory(UseItem);
        inventoryUI.SetInventory(inventory);
        ItemWorld.SpawnItemWorld(new Vector3(0, 0, 5), new Item { itemSO = inventoryUI.solarPanelSO, amount = 4 });
        ItemWorld.SpawnItemWorld(new Vector3(0, 0, 8), new Item { itemSO = inventoryUI.solarPanelSO, amount = 1 });
        ItemWorld.SpawnItemWorld(Vector3.one * 2, new Item { itemSO = inventoryUI.pickAxSO, amount = 1 });

    }
    private void Update()
    {
        Vector3 rotation = playerVisual.rotation.eulerAngles;
        rotation.y = camera.transform.rotation.eulerAngles.y;
        playerVisual.rotation = Quaternion.Euler(rotation);
        Debug.Log(inventory.GetItemList().Count);
    }
    public Inventory GetPlayerInventory()
    {
        return inventory;
    }
    private void AddToInventory(ItemWorld itemWorld)
    {
        inventory.AddItem(itemWorld.GetItem());
        itemWorld.DestroySelf();
    }
    private void UseItem(Item item)
    {
        switch (item.GetItemType())
        {
            case ItemType.MRE: break;
            case ItemType.SolarPanel:
                inventory.RemoveItem(new Item { itemSO = item.itemSO, amount = 1 });
                break;
            case ItemType.Rock: break;
            case ItemType.Ore: break;
        }
    }
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.TryGetComponent(out ItemWorld itemWorld))
    //     {
    //         AddToInventory(itemWorld);
    //     }
    // }
}