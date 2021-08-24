using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    Animator anim;

    [SerializeField] GameObject loadingScreen;
    [SerializeField] Slider loadingSlider;

    //Parents - enable or disable group of buttons
    [SerializeField] private GameObject whiteboardButtons;
    [SerializeField] private GameObject settingsButtons;
    [SerializeField] private GameObject gamemodeButtons;

    //Clicked Buttons
    [SerializeField] private Button modeButton;
    [SerializeField] private Button modesReturnButton;

    [SerializeField] private Button settingsButton;
    [SerializeField] private Button settingsReturnButton;

    //Quit Game
    [SerializeField] private GameObject quitGame;

    bool settingsBool = false;
    bool modesBool = false;
    bool whiteboardBool = false;
    bool modeToWhiteboard = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = FindObjectOfType<Animator>();

        loadingScreen.SetActive(false);
        
        //Buttons - Camera Animations
        if(modeButton)//Whiteboard to mode screen
            modeButton.onClick.AddListener(ChooseMode);
    
        if(settingsButton)
            settingsButton.onClick.AddListener(Settings);

        if(modesReturnButton)
            modesReturnButton.onClick.AddListener(ChooseModeReturn);

        if (settingsReturnButton)
            settingsReturnButton.onClick.AddListener(SettingsReturn);

        //Buttons Load Scenes
  
        


        whiteboardButtons.SetActive(false);
        gamemodeButtons.SetActive(false);
        settingsButtons.SetActive(false);

        //Intro camera animation
        whiteboardBool = true;
        StartCoroutine(WaitTime(6f));


    }

    IEnumerator WaitTime(float wait)
    {
        yield return new WaitForSeconds(wait);

        if(whiteboardBool)
        {
            whiteboardButtons.SetActive(true);
            whiteboardBool = false;
        }

        if(modesBool)
        {
            gamemodeButtons.SetActive(true);
        }

        if (settingsBool)
        {
            settingsButtons.SetActive(true);
        }

        if(modeToWhiteboard)
        {
            whiteboardButtons.SetActive(true);
        }
    }

    void ChooseMode()
    {
        //Trigger camera animation
        anim.SetTrigger("Modes");

        //Disable buttons
        whiteboardButtons.SetActive(false);

        //Toggle Booleans
        settingsBool = false;
        settingsBool = false;
        modesBool = true;
        whiteboardBool = false;
        modeToWhiteboard = false;

        //Wait to toggle buttons after animation
        StartCoroutine(WaitTime(3f));

       
    }
    //Camera animations
    void ChooseModeReturn()
    {
        //Trigger camera animation
        anim.SetTrigger("ModeToWhiteboard");

        //Disable buttons
        gamemodeButtons.SetActive(false);

        //Toggle Booleans
        settingsBool = false;
        settingsBool = false;
        modesBool = false;
        whiteboardBool = true;
        modeToWhiteboard = false;

        //Wait to toggle buttons after animation
        StartCoroutine(WaitTime(3f));

        
    }

    void Settings()
    {
        //Trigger camera animation
        anim.SetTrigger("Settings");

        //Disable buttons
        whiteboardButtons.SetActive(false);
        gamemodeButtons.SetActive(false);

        //Toggle Booleans
        settingsBool = true;
        modesBool = false;
        whiteboardBool = false;
        modeToWhiteboard = false;

        //Wait to toggle buttons after animation
        StartCoroutine(WaitTime(5f));
    }

    void SettingsReturn()
    {
        anim.SetTrigger("SettingsToWhiteboard");

        settingsButtons.SetActive(false);

        //Toggle Booleans
        settingsBool = false;
        settingsBool = false;
        modesBool = false;
        whiteboardBool = true;
        modeToWhiteboard = false;

        //Wait to toggle buttons after animation
        StartCoroutine(WaitTime(3.5f));
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    //Button Methods
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingSlider.value = progress;

            yield return null;
        }
        
    }

    
}
