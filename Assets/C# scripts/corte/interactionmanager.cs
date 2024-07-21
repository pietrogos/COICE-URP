using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;
    private bool isDragging = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsDragging()
    {
        return isDragging;
    }

    public void SetDragging(bool dragging)
    {
        isDragging = dragging;
    }
}