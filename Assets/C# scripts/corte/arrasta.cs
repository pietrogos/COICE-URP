using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private float zCoord;
    private Transform draggedObject;
    private Rigidbody draggedRigidbody;
    public LayerMask draggableLayer; // Layer mask for draggable objects

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
        }
        if (isDragging && draggedObject != null)
        {
            draggedObject.position = GetMouseWorldPos() + offset;
        }
    }

    void OnMouseDown()
    {
        if (InteractionManager.Instance.IsDragging())
        {
            return; // Prevent dragging if another drag operation is ongoing
        }

        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, draggableLayer))
        {
            draggedObject = hit.transform;
            draggedRigidbody = draggedObject.GetComponent<Rigidbody>();
            if (draggedRigidbody != null)
            {
                // Stop the object's movement
                draggedRigidbody.velocity = Vector3.zero;
                draggedRigidbody.angularVelocity = Vector3.zero;
                // Disable gravity and set kinematic to true to prevent physics interactions while dragging
                draggedRigidbody.isKinematic = true;
            }
            zCoord = mainCamera.WorldToScreenPoint(draggedObject.position).z;
            offset = draggedObject.position - GetMouseWorldPos();
            isDragging = true;
            InteractionManager.Instance.SetDragging(true);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        InteractionManager.Instance.SetDragging(false);
        if (draggedRigidbody != null)
        {
            // Re-enable gravity and set kinematic to false to allow physics interactions after dragging
            draggedRigidbody.isKinematic = false;
        }
        draggedObject = null;
        draggedRigidbody = null;
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}