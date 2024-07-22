using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static bool IsMainMenuActive = true; // Track if the main menu is active
    public GameObject crosshair; // Reference to the crosshair object
    public GameObject player; // Reference to the player object
    public GameObject mainMenu;

    void Start()
    {
        // Initialize the main menu state
        IsMainMenuActive = true;
        SetCursorVisible(true);
        Debug.Log("Main Menu Active: " + IsMainMenuActive);
    }

    public void StartGame()
    {
        IsMainMenuActive = false;
        SetCursorVisible(false);
        crosshair.SetActive(true);
        player.SetActive(true); // Enable player when starting the game
        mainMenu.SetActive(false);
        Debug.Log("Game Started - Main Menu Active: " + IsMainMenuActive);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void SetCursorVisible(bool visible)
    {
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visible;
        Debug.Log("Cursor Visible: " + Cursor.visible + ", Cursor Lock State: " + Cursor.lockState);
    }
}