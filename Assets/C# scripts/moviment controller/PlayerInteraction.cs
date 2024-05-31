using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 100.0f;
    [SerializeField] private float throwStrength = 10.0f;

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("UI Elements")]
    [SerializeField] private Text interactPrompt;
    [SerializeField] private GameObject crosshair;

    private InputAction interactAction;
    private InputAction throwAction;

    private GameObject heldObject = null;
    private Transform playerTransform;
    private bool isHoldingObject = false;

    private int interactableLayer = 7;

    private void Awake()
    {
        playerTransform = transform; // Use player's transform

        interactAction = playerControls.FindActionMap("Player").FindAction("Interact");
        throwAction = playerControls.FindActionMap("Player").FindAction("Throw");

        interactAction.performed += context => Interact();
        throwAction.performed += context => ThrowHeldObject();

        interactPrompt.text = ""; // Initialize prompt to empty

        interactableLayer = LayerMask.NameToLayer("Interactable");
    }

    private void OnEnable()
    {
        interactAction.Enable();
        throwAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
        throwAction.Disable();
    }

    private void Interact()
    {
        Debug.Log("Interact action triggered");
        if (heldObject && !isHoldingObject)
        {
            PickUpObject(heldObject);
        }
    }

    private void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        heldObject.transform.parent = Camera.main.transform; // Attach to camera
        heldObject.transform.localPosition = new Vector3(0, 0, 1); // Place in front
        heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
        isHoldingObject = true;
    }

    private void ThrowHeldObject()
    {
        if (isHoldingObject)
        {
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            rb.isKinematic = false; // Re-enable physics
            heldObject.transform.parent = null; // Detach from camera
            rb.AddForce(Camera.main.transform.forward * throwStrength, ForceMode.Impulse); // Throw object
            heldObject = null;
            isHoldingObject = false;
        }
    }

    private void NonGrabbableAction(GameObject target)
    {
        Debug.Log($"Interacting with non-grabbable object: {target.name}");
    }

    private void Update()
    {
        CheckForInteractableObjects();
    }

    private void CheckForInteractableObjects()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer == interactableLayer)
            {
                GameObject target = hit.collider.gameObject;
                if (!isHoldingObject)
                {
                    if (target.GetComponent<GrabbableObject>())
                    {
                        heldObject = target;
                        interactPrompt.text = "Press E to Interact";
                    }
                    else
                    {
                        NonGrabbableAction(target);
                    }
                }
            }
            else
            {
                interactPrompt.text = "";
                if (!isHoldingObject)
                {
                    heldObject = null;
                }
            }
        }
        else
        {
            interactPrompt.text = "";
            if (!isHoldingObject)
            {
                heldObject = null;
            }
        }
    }
}