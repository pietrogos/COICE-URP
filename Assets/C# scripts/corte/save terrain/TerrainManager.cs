using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class TreeInstanceData
{
    public float positionX;
    public float positionY;
    public float positionZ;
    public float widthScale;
    public float heightScale;
    public float rotation;
    public int prototypeIndex;
    public Color32 color;
    public Color32 lightmapColor;
}

[Serializable]
public class TerrainDataSerialized
{
    public List<TreeInstanceData> trees = new List<TreeInstanceData>();
}

public class TerrainManager : MonoBehaviour
{
    public Terrain terrain;
    private string filePath;

    private void InitializeFilePath()
    {
        filePath = Path.Combine(Application.persistentDataPath, "terrainData.json");
    }

    public void SaveTerrainData()
    {
        InitializeFilePath();
        TerrainDataSerialized data = new TerrainDataSerialized();

        foreach (TreeInstance tree in terrain.terrainData.treeInstances)
        {
            TreeInstanceData treeData = new TreeInstanceData
            {
                positionX = tree.position.x,
                positionY = tree.position.y,
                positionZ = tree.position.z,
                widthScale = tree.widthScale,
                heightScale = tree.heightScale,
                rotation = tree.rotation,
                prototypeIndex = tree.prototypeIndex,
                color = tree.color,
                lightmapColor = tree.lightmapColor
            };
            data.trees.Add(treeData);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Terrain data saved!");
    }

    public void LoadTerrainData()
    {
        InitializeFilePath();
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            TerrainDataSerialized data = JsonUtility.FromJson<TerrainDataSerialized>(json);

            TreeInstance[] treeInstances = new TreeInstance[data.trees.Count];
            for (int i = 0; i < data.trees.Count; i++)
            {
                TreeInstanceData treeData = data.trees[i];
                TreeInstance treeInstance = new TreeInstance
                {
                    position = new Vector3(treeData.positionX, treeData.positionY, treeData.positionZ),
                    widthScale = treeData.widthScale,
                    heightScale = treeData.heightScale,
                    rotation = treeData.rotation,
                    prototypeIndex = treeData.prototypeIndex,
                    color = treeData.color,
                    lightmapColor = treeData.lightmapColor
                };
                treeInstances[i] = treeInstance;
            }

            terrain.terrainData.treeInstances = treeInstances;

            // Ensure colliders are set up
            SetupTreeColliders();

            Debug.Log("Terrain data loaded!");
        }
        else
        {
            Debug.LogWarning("No saved terrain data found!");
        }
    }

    private void SetupTreeColliders()
    {
        TreePrototype[] treePrototypes = terrain.terrainData.treePrototypes;

        foreach (TreeInstance tree in terrain.terrainData.treeInstances)
        {
            TreePrototype treePrototype = treePrototypes[tree.prototypeIndex];
            GameObject treePrefab = treePrototype.prefab;

            // Assuming the prefab has a collider
            if (treePrefab.TryGetComponent(out Collider collider))
            {
                collider.enabled = true;
            }
        }
    }
    void Start()
    {
        LoadTerrainData();
        SetupTreeColliders();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // Change key if needed
        {
            SaveTerrainData();
        }

        if (Input.GetKeyDown(KeyCode.Y)) // Change key if needed
        {
            LoadTerrainData();
        }
    }
}