using UnityEngine;

public class RestoreTreeState : MonoBehaviour
{
    private Terrain terrain;

    void Start()
    {
        terrain = GetComponent<Terrain>();

        if (PlayerPrefs.HasKey("TreeCount"))
        {
            RestoreTreesFromPlayerPrefs();
        }
    }

    void RestoreTreesFromPlayerPrefs()
    {
        int treeCount = PlayerPrefs.GetInt("TreeCount");
        TreeInstance[] trees = new TreeInstance[treeCount];

        for (int i = 0; i < treeCount; i++)
        {
            trees[i].position = new Vector3(
                PlayerPrefs.GetFloat("Tree" + i + "PosX"),
                PlayerPrefs.GetFloat("Tree" + i + "PosY"),
                PlayerPrefs.GetFloat("Tree" + i + "PosZ")
            );

            trees[i].widthScale = PlayerPrefs.GetFloat("Tree" + i + "WidthScale");
            trees[i].heightScale = PlayerPrefs.GetFloat("Tree" + i + "HeightScale");
            trees[i].prototypeIndex = PlayerPrefs.GetInt("Tree" + i + "PrototypeIndex");
        }

        terrain.terrainData.treeInstances = trees;
    }
}