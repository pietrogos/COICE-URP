using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 100.0f;
    [SerializeField] private LayerMask interactableLayerMask;

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("UI Elements")]
    [SerializeField] private Text interactPrompt;
    [SerializeField] private GameObject crosshair;

    [Header("Prefabs")]
    [SerializeField] private GameObject woodPrefab;

    [Header("Throw Settings")]
    [SerializeField] private float throwStrength = 10f;
    [SerializeField] private float throwUpwardAngle = 20f;
    [SerializeField] private float minThrowVelocity = 2f;
    [SerializeField] private float throwDuration = 0.1f;

    [Header("Hold Settings")]
    [SerializeField] private Vector3 holdOffset = new Vector3(0, 0, 2f);

    public bool IsHoldingObject => isHoldingObject;

    private InputAction interactAction;
    private InputAction throwAction;

    private GameObject heldObject = null;
    private Transform playerTransform;
    private bool isHoldingObject = false;

    private GameObject currentTarget = null;

    private void Awake()
    {
        playerTransform = transform;

        interactAction = playerControls.FindActionMap("Player").FindAction("Interact");
        throwAction = playerControls.FindActionMap("Player").FindAction("Throw");

        interactAction.performed += context => Interact();
        //throwAction.performed += context => ThrowHeldObject();

        interactPrompt.text = "";
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
        heldObject = obj;
        heldObject.transform.SetParent(transform);
        SetHeldObjectProperties(heldObject);
       
        isHoldingObject = true;
    }

    private void SetHeldObjectProperties(GameObject obj)
    {
        obj.transform.localPosition = holdOffset;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rb.mass = 1f;
        rb.drag = 0f;
        rb.angularDrag = 0.05f;
        rb.useGravity = true;
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
                MusicManager.Instance.FastForwardMusic(100);
                break;
        }
    }

    private IEnumerator PressAnimation(GameObject button)
    {
        Vector3 initialPosition = button.transform.position;
        Vector3 pressedPosition = initialPosition + new Vector3(0, -0.05f, 0);

        button.transform.position = pressedPosition;
        yield return new WaitForSeconds(0.5f);

        button.transform.position = initialPosition;
    }
    //public void UpdateHeldObjectPosition()
    //{
    //    if (isHoldingObject && heldObject != null)
    //    {
    //        heldObject.transform.position = transform.position + transform.rotation * holdOffset;
    //    }
    //}

    //public void UpdateHeldObjectRotation()
    //{
    //    if (isHoldingObject && heldObject != null)
    //    {
    //        heldObject.transform.rotation = transform.rotation;
    //    }
    //}

    //private void ThrowHeldObject()
    //{
    //    if (!isHoldingObject || heldObject == null) return;

    //    // Release the object
    //    heldObject.transform.SetParent(null);
    //    Rigidbody rb = heldObject.GetComponent<Rigidbody>();
    //    rb.isKinematic = false;
    //    rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

    //    // Calculate throw direction and position
    //    Vector3 throwDirection = Camera.main.transform.forward;
    //    Vector3 throwPosition = Camera.main.transform.position + throwDirection * holdOffset.z;

    //    // Set object position and velocity
    //    heldObject.transform.position = throwPosition;
    //    rb.velocity = throwDirection * throwStrength;

    //    // Reset holding state
    //    heldObject = null;
    //    isHoldingObject = false;

    //    StartCoroutine(CheckThrowCollisions(rb));
    //}

    //private IEnumerator CheckThrowCollisions(Rigidbody rb)
    //{
    //    float checkDuration = 0.5f; // Check for 0.1 seconds
    //    float elapsedTime = 0f;

    //    while (elapsedTime < checkDuration)
    //    {
    //        if (rb.velocity.magnitude < 0.1f) // If the object has almost stopped
    //        {
    //            Debug.Log("Thrown object collided and stopped quickly");
    //            // You can add additional logic here if needed
    //            break;
    //        }
    //        elapsedTime += Time.fixedDeltaTime;
    //        yield return new WaitForFixedUpdate();
    //    }
    //}

    private void FixedUpdate()
    {
        CheckForInteractableObjects();
    }

    private void CheckForInteractableObjects()
    {
        RaycastHit hit;
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactionDistance, Color.red);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance, interactableLayerMask))
        {
            GameObject target = hit.collider.gameObject;
            if (!isHoldingObject)
            {
                if (target.GetComponent<GrabbableObject>())
                {
                    //if (currentTarget != target)
                    //{
                    //    currentTarget = target;
                    //    interactPrompt.text = "Pressione E para interagir";
                    //}
                }
                else
                {
                    if (target.name == "Play" || target.name == "Pause" || target.name == "FastForward")
                    {
                        currentTarget = target;
                        interactPrompt.text = "Press E to DJ";
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