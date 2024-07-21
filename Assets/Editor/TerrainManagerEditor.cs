using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainManager))]
public class TerrainManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainManager terrainManager = (TerrainManager)target;

        if (GUILayout.Button("Save Terrain Data"))
        {
            terrainManager.SaveTerrainData();
        }

        if (GUILayout.Button("Load Terrain Data"))
        {
            terrainManager.LoadTerrainData();
        }
    }
}