using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlayer : MonoBehaviour
{
    private GameObject player;

    void Awake()
    {
        // Find all objects of type GameObject, including inactive ones
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        // Iterate through the array to find the "Player" game object
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "player")
            {
                player = obj;
                break;
            }
        }

        // Check if the player object was found
        if (player == null)
        {
            Debug.LogError("Player game object not found in the scene.");
        }
    }

    // Method to enable the player game object
    public void EnablePlayerGameObject()
    {
        if (player != null)
        {
            player.SetActive(true);
            Debug.Log("Player enabled!");
        }
    }

    public void DisableMenu()
    {
        gameObject.SetActive(false);
    }
}

