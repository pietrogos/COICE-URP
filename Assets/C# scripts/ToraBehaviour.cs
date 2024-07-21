using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToraBehaviour : MonoBehaviour
{
    private float lifespan = 20.0f;
    private bool isInsideFire = false;
    private bool isInsideSpawn = false;
    private FireController fireController;
    private WoodSpawner woodSpawner;
    private AudioSource audioSource;
    public GameObject sparkleEffectPrefab;
    

    private void Start() 
    {
        fireController = FindObjectOfType<FireController>();
        woodSpawner = FindObjectOfType<WoodSpawner>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!isInsideSpawn)
        {
            lifespan -= Time.deltaTime;
        }

        if (lifespan <= 0)
        {
            woodSpawner.DecreaseToraCount();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spawn"))
        {
            isInsideSpawn = true;
        }
        if (other.CompareTag("FogueiraCollider"))
        {
            isInsideFire = true;
            Instantiate(sparkleEffectPrefab, transform.position, Quaternion.identity);
            if (audioSource && audioSource.clip)
            {
                audioSource.Play();
            }
            fireController.OnWoodAdded();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spawn"))
        {
            isInsideSpawn = false;
        }
        if (other.CompareTag("FogueiraCollider"))
        {
            isInsideFire = false;
        }
    }
}
