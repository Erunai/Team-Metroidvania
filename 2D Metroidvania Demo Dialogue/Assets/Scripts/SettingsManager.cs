using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;

public class SettingsManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    public AudioMixer audioMixer;


    private void Start()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow; // Set to FullScreenWindow
    }
    // Volume slider:
    public void SetVolume(float volume)
    {
        if (volume <= -79.5f)
        {
            audioMixer.SetFloat("volume", -80f); // Mute audio
        }
        else audioMixer.SetFloat("volume", volume);

        Debug.Log("Volume set to: " + volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); // must have quality index set up in the editor
        Debug.Log("Quality index set to: " + QualitySettings.GetQualityLevel());
        // Maybe will add resolution settings in future
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Debug.Log("Fullscreen set to: " + isFullScreen);
        Screen.fullScreen = isFullScreen;
    }

    public void SaveChanges()
    {
        // Save changes to settings here
        Debug.Log("Settings saved");
        // Close settings menu and return to pause menu
        QuitSettings();
    }
    public void UnsavedChanges()
    {
        // Show a warning message about unsaved changes
        Debug.Log("PLACEHODLER: You have unsaved changes. Do you want to save before quitting?");
        // Implement logic to show a confirmation dialog here

        // Quick back to pause menu
        QuitSettings();
    }

    private void QuitSettings() // Quit Settings and return to Pause menu
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

}
