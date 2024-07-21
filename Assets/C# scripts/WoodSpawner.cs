using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpawner : MonoBehaviour
{
    public GameObject toraPrefab; // Set this in the Inspector with your tora prefab
    public int maxToras = 20; // Maximum number of tora pieces allowed in the scene at once
    private int currentToras = 15; // Current number of active tora pieces

    private void Start()
    {
        StartCoroutine(SpawnToras());
    }

    IEnumerator SpawnToras()
    {
        while (true) // Infinite loop, be careful with these!
        {
            // Only spawn a new tora piece if we haven't reached the maximum number
            if (currentToras < maxToras)
            {
                // Spawn at the position of this GameObject
                Vector3 spawnPosition = transform.position;
                Instantiate(toraPrefab, spawnPosition, Quaternion.identity);
                currentToras++;
            }

            // Wait for 1 second before the next loop iteration
            yield return new WaitForSeconds(3);
        }
    }

    public void DecreaseToraCount()
    {
        if (currentToras > 0)
            currentToras--;
    }
}
