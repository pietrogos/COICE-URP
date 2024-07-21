using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cortecolisaot : MonoBehaviour
{
    // Reference to the chopped tree prefab
    public GameObject choppedTreePrefab;

    // The maximum distance to detect a tree from the collision point
    public float detectionRadius = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object is part of the terrain
        Terrain terrain = other.GetComponent<Terrain>();
        if (terrain != null)
        {
            // Get the collision point
            Vector3 collisionPoint = transform.position;

            // Find the tree at the collision point
            TerrainData terrainData = terrain.terrainData;
            TreeInstance[] trees = terrainData.treeInstances;

            for (int i = 0; i < trees.Length; i++)
            {
                Vector3 treePosition = Vector3.Scale(trees[i].position, terrainData.size) + terrain.transform.position;

                // Check if the tree position is near the collision point
                if (Vector3.Distance(treePosition, collisionPoint) < detectionRadius)
                {
                    // Instantiate the chopped tree prefab at the tree position
                    Vector3 choppedTreePosition = treePosition;
                    Quaternion choppedTreeRotation = Quaternion.identity;
                    GameObject choppedTree = Instantiate(choppedTreePrefab, choppedTreePosition, choppedTreeRotation);

                    // Set the scale of the chopped tree to match the destroyed tree
                    choppedTree.transform.localScale = trees[i].widthScale * Vector3.one;

                    // Remove the tree from the terrain
                    trees[i] = trees[trees.Length - 1];
                    System.Array.Resize(ref trees, trees.Length - 1);
                    terrainData.treeInstances = trees;

                    // Break the loop since the tree is found and processed
                    break;
                }
            }
        }
    }
}