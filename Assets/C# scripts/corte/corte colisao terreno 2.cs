using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cortecolisaoterreno2 : MonoBehaviour
{
    public GameObject choppedTreePrefab;
    public float detectionRadius = 1.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(ChopTreeWithDelay());
        }
    }

    private IEnumerator ChopTreeWithDelay()
    {
        yield return new WaitForSeconds(1.0f);
        ChopTree();
        Debug.Log("ChopTree start");
    }

    private void ChopTree()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            Terrain terrain = hitCollider.GetComponent<Terrain>();
            if (terrain != null)
            {
                Vector3 collisionPoint = transform.position;
                TerrainData terrainData = terrain.terrainData;
                TreeInstance[] trees = terrainData.treeInstances;

                for (int i = 0; i < trees.Length; i++)
                {
                    Vector3 treePosition = Vector3.Scale(trees[i].position, terrainData.size) + terrain.transform.position;
                    if (Vector3.Distance(treePosition, collisionPoint) < detectionRadius)
                    {
                        Vector3 choppedTreePosition = treePosition;
                        Quaternion choppedTreeRotation = Quaternion.identity;
                        GameObject choppedTree = Instantiate(choppedTreePrefab, choppedTreePosition, choppedTreeRotation);

                        choppedTree.transform.localScale = trees[i].widthScale * Vector3.one;

                        trees[i] = trees[trees.Length - 1];
                        System.Array.Resize(ref trees, trees.Length - 1);
                        terrainData.treeInstances = trees;

                        break;
                    }
                }
            }
        }
    }
}