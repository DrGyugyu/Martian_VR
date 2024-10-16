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
    [SerializeField] Transform playerVisual;
    [SerializeField] private Transform grabTranform;
    [SerializeField] XRRayInteractor rightHandInteractor;
    [SerializeField] public InputActionProperty XRMoveAction;
    [SerializeField] public InputActionProperty XRGrabAction;
    public Inventory inventory;
    [SerializeField] private InventoryUI inventoryUI;
    private void OnEnable()
    {
        XRMoveAction.action.performed += (ctx) =>
        {
            //Debug.Log(ctx.ReadValue<Vector2>());
            animator.SetFloat(hashMovement, ctx.ReadValue<Vector2>().magnitude);
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
        inventory = new Inventory();
        inventoryUI.SetInventory(inventory);
        //playerVisual = Transform.FindFirstObjectByType<Transform>();
    }
    private void Update()
    {
        Vector3 rotation = playerVisual.rotation.eulerAngles;
        rotation.y = camera.transform.rotation.eulerAngles.y;
        playerVisual.rotation = Quaternion.Euler(rotation);
    }

}