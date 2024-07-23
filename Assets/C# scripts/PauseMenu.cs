using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject audioOptionsUI;
    public GameObject controlsUI;
    public Slider generalVolumeSlider;
    public Slider musicVolumeSlider;
    public GameObject crosshair; // Reference to the crosshair object
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script
    private bool isPaused = false;

    void Start()
    {
        generalVolumeSlider.onValueChanged.AddListener(SetGeneralVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

        // Ensure that only the pause menu is active initially
        pauseMenuUI.SetActive(false);
        audioOptionsUI.SetActive(false);
        controlsUI.SetActive(false);
        HideCursor();
    }

    void Update()
    {
        if ((Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            if (!MainMenuManager.IsMainMenuActive)
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        audioOptionsUI.SetActive(false);
        controlsUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        crosshair.SetActive(true);
        playerMovement.EnableMovement(); // Reativar o controle da câmera
        HideCursor();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        audioOptionsUI.SetActive(false);
        controlsUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        crosshair.SetActive(false);
        playerMovement.DisableMovement(); // Desativar o controle da câmera
        ShowCursor();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenControls()
    {
        pauseMenuUI.SetActive(false);
        controlsUI.SetActive(true);
    }

    public void OpenAudioOptions()
    {
        pauseMenuUI.SetActive(false);
        audioOptionsUI.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        audioOptionsUI.SetActive(false);
        controlsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void SetGeneralVolume(float volume)
    {
        // Implement the logic to set the general volume
    }

    void SetMusicVolume(float volume)
    {
        // Implement the logic to set the music volume
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Cursor Shown: " + Cursor.visible);
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Cursor Hidden: " + Cursor.visible);
    }
}
