using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject audioOptionsUI;
    public GameObject controlsUI;
    public GameObject videoOptionsUI;
    public Slider generalVolumeSlider;
    public Slider musicVolumeSlider;
    public GameObject crosshair; // Reference to the crosshair object
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script
    public AudioSource musicSource;
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Dropdown qualityDropdown;

    private Resolution[] resolutions;
    private bool isPaused = false;

    void Start()
    {
        generalVolumeSlider.onValueChanged.AddListener(SetGeneralVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

        // Populate resolution dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (Resolution resolution in resolutions)
        {
            options.Add(resolution.width + " x " + resolution.height);
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.RefreshShownValue();

        // Initialize fullscreen and quality settings
        fullscreenToggle.isOn = Screen.fullScreen;

        qualityDropdown.ClearOptions();
        List<string> qualityOptions = new List<string>(QualitySettings.names);
        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();

        // Add listeners for video options
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        qualityDropdown.onValueChanged.AddListener(SetQuality);

        // Ensure that only the pause menu is active initially
        pauseMenuUI.SetActive(false);
        audioOptionsUI.SetActive(false);
        controlsUI.SetActive(false);
        videoOptionsUI.SetActive(false);
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
        videoOptionsUI.SetActive(false);
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
        videoOptionsUI.SetActive(false);
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
    public void OpenVideoOptions()
    {
        pauseMenuUI.SetActive(false);
        videoOptionsUI.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        audioOptionsUI.SetActive(false);
        controlsUI.SetActive(false);
        videoOptionsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetGeneralVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
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

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return 0;
    }
}
