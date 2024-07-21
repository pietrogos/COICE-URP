using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject audioOptionsUI;
    public GameObject controlsUI;
    public Slider generalVolumeSlider;
    public Slider musicVolumeSlider;
    public GameObject player; // Reference to the player object
    public GameObject crosshair; // Reference to the crosshair object

    private bool isPaused = false;
    private PlayerMovement playerController; // Assuming this is your player control script

    void Start()
    {
        playerController = player.GetComponent<PlayerMovement>();
        generalVolumeSlider.onValueChanged.AddListener(SetGeneralVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

        // Ensure that only the pause menu is active initially
        pauseMenuUI.SetActive(false);
        audioOptionsUI.SetActive(false);
        controlsUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if ((Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
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

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        audioOptionsUI.SetActive(false);
        controlsUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        playerController.enabled = true;
        crosshair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        audioOptionsUI.SetActive(false);
        controlsUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        playerController.enabled = false;
        crosshair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
}
