using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class clickreset : MonoBehaviour
{
    public KeyCode reloadKey = KeyCode.R; // Set the key you want to use to reload the scene

    void Update()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            ReloadCurrentScene();
        }
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
