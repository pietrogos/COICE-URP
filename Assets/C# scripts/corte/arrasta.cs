using UnityEngine;

public class DragObject : MonoBehaviour
{
    public float force = 600;
    public float damping = 6;
    public float dragDistance = 2.0f; // Distance from the player during dragging
    public LayerMask draggableLayer;  // Layer mask for draggable objects

    private Transform jointTrans;
    private Transform playerCamera;
    private Rigidbody draggedRigidbody;
    private bool isDragging = false;
    private float dragDepth;
    private Vector3 offset;

    void Start()
    {
        playerCamera = Camera.main.transform; // Assuming the main camera is the player's camera
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isDragging)
            {
                HandleInputEnd();
            }
            else
            {
                HandleInputBegin();
            }
        }

        if (isDragging && jointTrans != null)
        {
            HandleInput();
        }
    }

    public void HandleInputBegin()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, draggableLayer))
        {
            dragDepth = Vector3.Distance(playerCamera.position, hit.point);
            jointTrans = AttachJoint(hit.rigidbody, hit.point);
            isDragging = true;
        }
    }

    public void HandleInput()
    {
        if (jointTrans == null)
            return;

        Vector3 worldPos = playerCamera.position + playerCamera.forward * dragDistance;
        jointTrans.position = worldPos;
    }

    public void HandleInputEnd()
    {
        if (jointTrans != null)
        {
            Destroy(jointTrans.gameObject);
            jointTrans = null;
            isDragging = false;
        }
    }

    Transform AttachJoint(Rigidbody rb, Vector3 attachmentPosition)
    {
        GameObject go = new GameObject("Attachment Point");
        go.hideFlags = HideFlags.HideInHierarchy;
        go.transform.position = attachmentPosition;

        var newRb = go.AddComponent<Rigidbody>();
        newRb.isKinematic = true;

        var joint = go.AddComponent<ConfigurableJoint>();
        joint.connectedBody = rb;
        joint.configuredInWorldSpace = true;
        joint.xDrive = NewJointDrive(force, damping);
        joint.yDrive = NewJointDrive(force, damping);
        joint.zDrive = NewJointDrive(force, damping);
        joint.slerpDrive = NewJointDrive(force, damping);
        joint.rotationDriveMode = RotationDriveMode.Slerp;

        return go.transform;
    }

    private JointDrive NewJointDrive(float force, float damping)
    {
        JointDrive drive = new JointDrive();
        drive.positionSpring = force;
        drive.positionDamper = damping;
        drive.maximumForce = Mathf.Infinity;
        return drive;
    }
}