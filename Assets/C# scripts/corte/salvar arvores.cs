using UnityEngine;

public class SaveInitialTreeState : MonoBehaviour
{
    private Terrain terrain;
    private TreeInstance[] initialTrees;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        initialTrees = terrain.terrainData.treeInstances;

        // Save the initial tree states to PlayerPrefs
        SaveTreesToPlayerPrefs();
    }

    void SaveTreesToPlayerPrefs()
    {
        for (int i = 0; i < initialTrees.Length; i++)
        {
            PlayerPrefs.SetFloat("Tree" + i + "PosX", initialTrees[i].position.x);
            PlayerPrefs.SetFloat("Tree" + i + "PosY", initialTrees[i].position.y);
            PlayerPrefs.SetFloat("Tree" + i + "PosZ", initialTrees[i].position.z);
            PlayerPrefs.SetFloat("Tree" + i + "WidthScale", initialTrees[i].widthScale);
            PlayerPrefs.SetFloat("Tree" + i + "HeightScale", initialTrees[i].heightScale);
            PlayerPrefs.SetInt("Tree" + i + "PrototypeIndex", initialTrees[i].prototypeIndex);
        }

        PlayerPrefs.SetInt("TreeCount", initialTrees.Length);
        PlayerPrefs.Save();
    }
}