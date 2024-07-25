using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegarMachado : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    private GameObject machado; // Reference to the machado object
    private Animationcontroler animationController; // Reference to the Animationcontroler script

    private void Start()
    {
        // Find and reference the "macchado 1" GameObject
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "macchado 1")
            {
                machado = obj;
                animationController = machado.GetComponent<Animationcontroler>();
                break; // Exit the loop once machado is found
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ensure the machado is found and set
        if (other.gameObject == player && machado != null)
        {
            machado.SetActive(true); // Enable the machado object
            if (animationController != null)
            {
                animationController.enabled = true; // Ensure the Animationcontroler script is enabled
            }
            Destroy(gameObject); // Destroy the object this script is attached to
        }
    }
}
