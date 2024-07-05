using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrasta : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody selectedObject;
    private Vector3 offset;
    private float zCoordinate;

    // Dragging parameters
    public float dragSpeed = 10f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Change this to 1 for right mouse button, or use KeyCode for keyboard input
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("ChoppedTree"))
                {
                    selectedObject = hit.collider.GetComponent<Rigidbody>();
                    zCoordinate = mainCamera.WorldToScreenPoint(selectedObject.transform.position).z;
                    offset = selectedObject.transform.position - hit.point;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObject != null) // Change this to 1 for right mouse button, or use KeyCode for keyboard input
        {
            selectedObject = null;
        }

        if (selectedObject != null)
        {
            Vector3 targetPosition = GetMouseWorldPosition() + offset;
            selectedObject.MovePosition(Vector3.Lerp(selectedObject.position, targetPosition, dragSpeed * Time.deltaTime));
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoordinate;

        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}