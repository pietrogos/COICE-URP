using UnityEngine;

public class EnablePlayer : MonoBehaviour
{
    private GameObject player;
    public GameObject crosshair; // Reference to the crosshair object

    void Awake()
    {
        player = GameObject.Find("player");

        if (player == null)
        {
            Debug.LogError("Player game object not found in the scene.");
        }
    }

    public void EnablePlayerGameObject()
    {
        if (player != null)
        {
            player.SetActive(true);
            crosshair.SetActive(true);
            Debug.Log("Player Enabled");
        }
    }

    public void DisablePlayerGameObject()
    {
        if (player != null)
        {
            player.SetActive(false);
            crosshair.SetActive(false);
            Debug.Log("Player Disabled");
        }
    }
}
