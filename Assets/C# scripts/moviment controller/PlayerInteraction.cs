using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 100.0f;
    [SerializeField] private float throwStrength = 10.0f;
    [SerializeField] private LayerMask interactableLayerMask;

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("UI Elements")]
    [SerializeField] private Text interactPrompt;
    [SerializeField] private GameObject crosshair;

    [Header("Prefabs")]
    [SerializeField] private GameObject woodPrefab;

    private InputAction interactAction;
    private InputAction throwAction;

    private GameObject heldObject = null;
    private Transform playerTransform;
    private bool isHoldingObject = false;

    private GameObject currentTarget = null;

    private void Awake()
    {
        playerTransform = transform; // Use player's transform

        interactAction = playerControls.FindActionMap("Player").FindAction("Interact");
        throwAction = playerControls.FindActionMap("Player").FindAction("Throw");

        interactAction.performed += context => Interact();
        throwAction.performed += context => ThrowHeldObject();

        interactPrompt.text = ""; // Initialize prompt to empty
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
        if (currentTarget && !isHoldingObject)
        {
            if (currentTarget.name == "PileOfWood")
            {
                PickUpObject(currentTarget);
            }
            else
            {
                NonGrabbableAction(currentTarget);
            }
        }
    }

    private void PickUpObject(GameObject obj)
    {
        if (obj.name == "PileOfWood")
        {
            // Spawn a new wood object
            GameObject woodObject = Instantiate(woodPrefab, Camera.main.transform);
            woodObject.transform.localPosition = new Vector3(0, 0, 1); // Place in front
            woodObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
            heldObject = woodObject;
            isHoldingObject = true;
        }
        else
        {
            // Handle other objects if necessary
            heldObject = obj;
            heldObject.transform.parent = Camera.main.transform; // Attach to camera
            heldObject.transform.localPosition = new Vector3(0, 0, 5); // Place in front
            heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
            isHoldingObject = true;
        }
    }

    private void NonGrabbableAction(GameObject target)
    {
        Debug.Log($"Interacting with non-grabbable object: {target.name}");

        if (target.name == "Play" || target.name == "Pause" || target.name == "FastForward")
        {
            DJButtonAction(target);
        }
    }

    private void DJButtonAction(GameObject button)
    {
        StartCoroutine(PressAnimation(button));
        switch (button.name)
        {
            case "Play":
                MusicManager.Instance.PlayMusic();
                break;
            case "Pause":
                MusicManager.Instance.PauseMusic();
                break;
            case "FastForward":
                MusicManager.Instance.FastForwardMusic(100); //100 seconds
                break;
        }
    }

    private IEnumerator PressAnimation(GameObject button)
    {
        Vector3 initialPosition = button.transform.position;
        Vector3 pressedPosition = initialPosition + new Vector3(0, -0.05f, 0); // Move down

        // Move button down
        button.transform.position = pressedPosition;
        yield return new WaitForSeconds(0.5f);

        // Move button back up
        button.transform.position = initialPosition;
    }


    private void ThrowHeldObject()
    {
        if (isHoldingObject && heldObject != null)
        {
            // Detach the held object from the camera
            heldObject.transform.parent = null;

            // Ensure the Rigidbody is not null and then apply the force
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Disable kinematic before applying force
                rb.isKinematic = false;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

                // Position the object slightly away from the player to avoid immediate collision
                Vector3 throwPosition = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                heldObject.transform.position = throwPosition;
                heldObject.transform.rotation = Camera.main.transform.rotation;

                // Apply the force in the forward direction of the camera
                Vector3 forceDirection = Camera.main.transform.forward;
                forceDirection.y = Mathf.Max(forceDirection.y, 0.1f); // Ensure the y-component of the force is not negative
                rb.AddForce(forceDirection * throwStrength, ForceMode.Impulse);

                // Debugging information
                Debug.Log($"Object thrown with force: {throwStrength} in direction: {Camera.main.transform.forward}");
                Debug.Log($"Throw position: {throwPosition}");
                Debug.Log($"Rigidbody velocity after throw: {rb.velocity}");
            }

            // Reset held object variables
            heldObject = null;
            isHoldingObject = false;
        }
    }

    private void FixedUpdate()
    {
        CheckForInteractableObjects();
    }

    private void CheckForInteractableObjects()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance, interactableLayerMask))
        {
            GameObject target = hit.collider.gameObject;
            if (!isHoldingObject)
            {
                if (target.GetComponent<GrabbableObject>())
                {
                    if (currentTarget != target)
                    {
                        currentTarget = target;
                        interactPrompt.text = "Press E to Interact";
                    }
                }
                else
                {
                    if (target.name == "Play" || target.name == "Pause" || target.name == "FastForward")
                    {
                        currentTarget = target;
                        interactPrompt.text = "Press E to Interact";
                    }
                    else
                    {
                        NonGrabbableAction(target);
                        interactPrompt.text = "";
                        currentTarget = null;
                    }
                }
            }
        }
        else
        {
            interactPrompt.text = "";
            currentTarget = null;
        }
    }
}
