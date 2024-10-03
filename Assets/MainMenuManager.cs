using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static bool IsMainMenuActive = true; // Track if the main menu is active
    public GameObject crosshair; // Reference to the crosshair object
    public GameObject player; // Reference to the player object
    public GameObject mainMenu;

    void Awake()
    {
        // Initialize the main menu state
        IsMainMenuActive = true;
        SetCursorVisible(true);
        Cursor.visible = true;
        Screen.lockCursor = false;
        Debug.Log("Main Menu Active: " + IsMainMenuActive);
    }

    void Update()
    {
        // Check if the player presses "Enter" and the main menu is active
        if (IsMainMenuActive && Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
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