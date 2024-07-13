using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathCreator))]
public class PathCreatorEditor : Editor
{
    private PathCreator pathCreator;
    private bool isAddingPoints = false;

    private void OnEnable()
    {
        pathCreator = (PathCreator)target;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (isAddingPoints)
        {
            HandleAddingPoints();
        }
        HandlePointDragging();
    }

    private void HandleAddingPoints()
    {
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.GetComponent<Terrain>() == pathCreator.terrain)
                {
                    pathCreator.AddPoint(hit.point);
                    e.Use();
                }
            }
        }
    }

    private void HandlePointDragging()
    {
        for (int i = 0; i < pathCreator.pathPoints.Count; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newTargetPosition = Handles.PositionHandle(pathCreator.pathPoints[i], Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(pathCreator, "Move Point");
                pathCreator.pathPoints[i] = newTargetPosition;
                pathCreator.UpdatePath();
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Start Adding Points"))
        {
            isAddingPoints = true;
        }

        if (GUILayout.Button("Stop Adding Points"))
        {
            isAddingPoints = false;
        }

        if (GUILayout.Button("Clear Points"))
        {
            pathCreator.ClearPoints();
        }

        if (GUILayout.Button("Generate Path"))
        {
            pathCreator.GeneratePath();
        }

        if (GUILayout.Button("Smooth Terrain"))
        {
            pathCreator.SmoothTerrain();
        }
    }
}
