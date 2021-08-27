/*
 * 
 * Author @Aleksi Putkonen
 * Free to use xD
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown textureQualityDropdown;
 // public Dropdown AntiAliasingDropdown; <-- Use if antialiasing setting is added to the game!
    public Dropdown vSyncDropdown;
    public Slider musicVolumeSlider;
    public Button applyButton;

    public AudioSource musicSource;
    public Resolution[] resolutions;
    public GameSettings gameSettings;

    

    void OnEnable()
    {
        gameSettings = new GameSettings();

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
     // AntiAliasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });  <-- Use if antialiasing setting is added to the game!
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        //musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolume(); }); //KYS vittu
        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });
 
        resolutions = Screen.resolutions;
        foreach(Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        LoadSettings(); // This calls the load settings method
    }

    // Game changes to fullscreen mode
    public void OnFullscreenToggle()
    {
        gameSettings.fullScreen = Screen.fullScreen = fullscreenToggle.isOn;
    }

    // Game changes the resolution
    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }

    // Game changes the texture quality
    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
    }

    /*
    Use if antialiasing setting is added to the game!
    
    // Game changes the level of anti aliasing
    public void OnAntialiasingChange()
    {
        QualitySettings.antiAliasing = gameSettings.antialiasing = (int)Mathf.Pow(2f, AntiAliasingDropdown.value);
    } */

    // Game applies the selected VSync
    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
    }

    // Game applies the level of volume
    /*
    public void OnMusicVolume()
    {
        musicSource.volume = gameSettings.musicVolume = musicVolumeSlider.value;
    }
    */
    

    // Saves the game settings when apply is pressed
    public void OnApplyButtonClick()
    {
        SaveSettings();
    }

    // Saves the game method
    public void SaveSettings()
    {   // Saves the game settings on Json file (true to format everything)
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }
    
    /*
    // Check if there is save file -> skips settings loading if there isn't one
    if (File.Exists(Application.persistentDataPath + "/gamesettings.json") == true)
        {
            LoadSettings();
        }
    */

    // Loads the game method
    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        
        musicVolumeSlider.value = gameSettings.musicVolume;
        // antialiasingDropdown.value = gameSettings.antialiasing;  <-- Use if antialiasing setting is added to the game!
        vSyncDropdown.value = gameSettings.vSync;
        textureQualityDropdown.value = gameSettings.textureQuality;
        resolutionDropdown.value = gameSettings.resolutionIndex;
        fullscreenToggle.isOn = gameSettings.fullScreen;
        Screen.fullScreen = gameSettings.fullScreen;

        resolutionDropdown.RefreshShownValue(); // Shows the saved value

    }

}
