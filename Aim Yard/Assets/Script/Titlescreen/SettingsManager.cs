using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class SettingsData
{
        public int[] resolution;
        public int qualityLevel;
        public int displayMode;
    public SettingsData (int[] _resolution, int _qualityLevel, int _displayMode)
    {
        resolution = _resolution;
        qualityLevel = _qualityLevel;
        displayMode = _displayMode;
    }
}

public class SettingsManager : MonoBehaviour
{

    [SerializeField] private Button saveSettingsButton;
    [SerializeField] private Button loadSettingsButton;

    [SerializeField] private TMP_Dropdown qualityLevelDropDown;
    [SerializeField] private TMP_Dropdown displayModeDropDown;
    [SerializeField] private TMP_Dropdown resolutionDropDown;

    Resolution[] resolutions;
    List<string> qualityLevels;

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
        var convertedResolution = ConvertResolution(resolutionDropDown.options[selectedResolutionIdx].text);

        if (convertedResolution[0] == -1 || convertedResolution[1] == -1)
        {
            Debug.LogError("Error converting resolution");
            return;
        }
        var isFullscreen = Screen.fullScreen;
        int[] resolutionAndIdx = new int[3] { convertedResolution[0], convertedResolution[1], selectedResolutionIdx };

        SettingsData data = new SettingsData(resolutionAndIdx, qualityLevelDropDown.value, displayModeDropDown.value);
        SaveSystem.SaveSettings(data);

    }

    public void LoadSettings()
    {
        SettingsData data = SaveSystem.LoadSettings();
        if (data == null)
        {
            Debug.LogError("Setting data failed to load or file doesn't exist");
            return;
        }

        QualitySettings.SetQualityLevel(data.qualityLevel);
        Screen.SetResolution(data.resolution[0], data.resolution[1], true);
        Screen.fullScreenMode = (FullScreenMode)data.displayMode;

        qualityLevelDropDown.value = data.qualityLevel;
        resolutionDropDown.value = data.resolution[2];
        displayModeDropDown.value = data.displayMode;
    }

    private int[] ConvertResolution(string resolution)
    {
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
        displayModeDropDown.ClearOptions();

        var displayModeOptions = new List<string> { "Fullscreen", "Borderless Fullscreen", "Borderless Window", "Window" };
        displayModeDropDown.AddOptions(displayModeOptions);
    }

    private void GetQualityLevel()
    {
        qualityLevels = new List<string>(QualitySettings.names);
        var currentQuality = QualitySettings.GetQualityLevel();
        qualityLevelDropDown.ClearOptions();
        qualityLevelDropDown.AddOptions(qualityLevels);
        qualityLevelDropDown.value = currentQuality;
    }

    private void GetResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        var options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            var option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }
        resolutionDropDown.AddOptions(options);
    }
}
