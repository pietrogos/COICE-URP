using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cortecolisao : MonoBehaviour
{
    public GameObject choppedTreePrefab;

    // Tag of the tree game object
    public string treeTag = "arvore";

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the tree tag
        if (other.CompareTag(treeTag))
        {
            // Instantiate the chopped tree prefab at the same position and rotation as the tree
            Instantiate(choppedTreePrefab, other.transform.position, other.transform.rotation);

            // Destroy the tree object
            Destroy(other.gameObject);

            Debug.Log("crote");
        }
    }
}