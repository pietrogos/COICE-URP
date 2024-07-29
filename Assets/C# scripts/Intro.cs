using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    // Start is called before the first frame update
    private float temporizador = 5.5f; // Cooldown period in seconds
    private bool isCooldown = false; // Flag to check if cooldown is active
    public GameObject objectToEnable; // Reference to the GameObject to enable

    void Start()
    {
        StartCoroutine(StartCountdown());


    }

    private IEnumerator StartCountdown()
    {
        // Wait for the specified countdown time
        yield return new WaitForSeconds(temporizador);

        // Enable the specified GameObject
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }

        // Destroy the GameObject this script is attached to
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
