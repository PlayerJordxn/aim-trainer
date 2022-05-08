using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    // Buttons to save or load
    [SerializeField] private Button saveSettingsButton;
    [SerializeField] private Button loadSettingsButton;
    // TMP drop down objects
    [SerializeField] private TMP_Dropdown qualityLevelDropDown;
    [SerializeField] private TMP_Dropdown displayModeDropDown;
    [SerializeField] private TMP_Dropdown resolutionDropDown;

    Resolution[] resolutions;
    List<string> qualityLevels;

    /*
     * Retrieve all available stats on start to fill out our settings page and then display
     */
    void Start()
    {
        GetResolutions();
        GetDisplayMode();
        GetQualityLevel();
        saveSettingsButton.onClick.AddListener(SaveSettings);
        loadSettingsButton.onClick.AddListener(LoadSettings);
        LoadSettings();
    }

    void Update()
    {
        
    }

    public void SaveSettings()
    {

        var selectedResolutionIdx = resolutionDropDown.value;
        // This parses the resolution from a string into a int[]. [0] is x, [1] is y
        var convertedResolution = ConvertResolution(resolutionDropDown.options[selectedResolutionIdx].text);

        if (convertedResolution[0] == -1 || convertedResolution[1] == -1)
        {
            Debug.LogError("Error converting resolution");
            return;
        }
        int[] resolutionAndIdx = new int[3] { convertedResolution[0], convertedResolution[1], selectedResolutionIdx };

        // Create a new SettingsData object and save it.
        SettingsData data = new SettingsData(resolutionAndIdx, qualityLevelDropDown.value, displayModeDropDown.value);
        SaveSystem.SaveData(data);

    }

    public void LoadSettings()
    {
        // Load settings, if settings don't exist, we'll have a null value. In that case we return.
        SettingsData data = SaveSystem.LoadSettings();
        if (data == null)
        {
            Debug.LogError("Setting data failed to load or file doesn't exist");
            return;
        }
        // Set all of our settings from the .bin file
        QualitySettings.SetQualityLevel(data.qualityLevel);
        Screen.SetResolution(data.resolution[0], data.resolution[1], true);
        Screen.fullScreenMode = (FullScreenMode)data.displayMode;
        // Set all of our dropdowns to the correct values that are set
        qualityLevelDropDown.value = data.qualityLevel;
        resolutionDropDown.value = data.resolution[2];
        displayModeDropDown.value = data.displayMode;
    }

    private int[] ConvertResolution(string resolution)
    {
        // String comes in as "XXXX x YYYY" so we need to trim it to convert into an int[]
        var splitResolution = resolution.Split('x');
        int[] results = new int[2];

        for (var i=0; i<2; i++)
        {
            try
            {
                results[i] = int.Parse(splitResolution[i].Trim());
            }
            catch
            {
                results[i] = -1;
            }
        }

        return results;
    }

    private void GetDisplayMode()
    {
        // Clear current options in case we have any that are unexpected
        displayModeDropDown.ClearOptions();

        // **NOTE: Leave display options in this order, this is how Unity's enum for FullScreenMode orders them"
        var displayModeOptions = new List<string> { "Fullscreen", "Borderless Fullscreen", "Borderless Window", "Window" };
        displayModeDropDown.AddOptions(displayModeOptions);
    }

    private void GetQualityLevel()
    {
        // Grab all quality levels
        qualityLevels = new List<string>(QualitySettings.names);
        // Get current
        var currentQuality = QualitySettings.GetQualityLevel();
        // Clear old options
        qualityLevelDropDown.ClearOptions();
        // Add options
        qualityLevelDropDown.AddOptions(qualityLevels);
        // Update drowpdown to show our currently selected option
        qualityLevelDropDown.value = currentQuality;
    }

    private void GetResolutions()
    {
        // TMP Dropdowns only take string lists in for options, so we have to convert our selection into
        // something readable and selectable for the user.
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        var options = new List<string>();
        // iterate through all options
        for (int i = 0; i < resolutions.Length; i++)
        {
            // This gives us "XXXX x YYYY" which would be fine for users
            var option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }
        resolutionDropDown.AddOptions(options);
    }
}
