using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{

    //[SerializeField] private Camera gameCamera;
    [SerializeField] private Button saveSettingsButton;

    [SerializeField] private TMP_Dropdown gameResolutionDropDown;
    [SerializeField] private TMP_Dropdown aspectRatioDropDown;
    [SerializeField] private TMP_Dropdown displayModeDropDown;
    [SerializeField] private TMP_Dropdown resolutionDropDown;

    Resolution[] resolutions;
    List<string> gameResolutions;
    bool displayMode;


    //[SerializeField] private Button saveSettings;

    // Start is called before the first frame update
    void Start()
    {
        GetResolutions();
        GetDisplayMode();
        GetGameRes();
        saveSettingsButton.onClick.AddListener(SaveSettings);
        //saveSettings.onClick.AddListener(delegate { SaveSettings(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveSettings()
    {

        var selectedResolutionIdx = resolutionDropDown.value;
        var convertedResolution = ConvertResolution(resolutionDropDown.options[selectedResolutionIdx].text);

        if (convertedResolution[0] == -1 || convertedResolution[1] == -1)
        {
            Debug.Log("Error converting resolution");
            return;
        }
        var isFullscreen = Screen.fullScreen;
        Debug.Log(convertedResolution[0] + " " + convertedResolution[1]);

        QualitySettings.SetQualityLevel(gameResolutionDropDown.value);
        Screen.SetResolution(convertedResolution[0], convertedResolution[1], isFullscreen);
        

        Debug.Log("Game res is now " + QualitySettings.GetQualityLevel() + " res is " + Screen.currentResolution.ToString());

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

    //putting a pin in this one
    /*
    private void GetAspectRatios()
    {
        var ar = gameCamera.aspect;
        Debug.Log(ar + " aspect ratio");
    }
    */

    private void GetDisplayMode()
    {
        displayMode = Screen.fullScreen;
        displayModeDropDown.ClearOptions();

        var displayModeOptions = new List<string> { "Fullscreen", "Borderless Fullscreen", "Borderless Window", "Window" };
        displayModeDropDown.AddOptions(displayModeOptions);
    }

    private void GetGameRes()
    {
        gameResolutions = new List<string>(QualitySettings.names);
        var currentGameRes = QualitySettings.GetQualityLevel();
        gameResolutionDropDown.ClearOptions();
        gameResolutionDropDown.AddOptions(gameResolutions);
        gameResolutionDropDown.value = currentGameRes;
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
