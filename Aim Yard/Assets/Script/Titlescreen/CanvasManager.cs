using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    Animator anim;

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

    // Start is called before the first frame update
    void Start()
    {
        anim = FindObjectOfType<Animator>();

        //Buttons
        if(modeButton)//Whiteboard to mode screen
            modeButton.onClick.AddListener(ChooseMode);
    
    
        if(settingsButton)
            settingsButton.onClick.AddListener(Settings);

        if(modesReturnButton)
            modesReturnButton.onClick.AddListener(ChooseModeReturn);

        whiteboardButtons.SetActive(false);
        gamemodeButtons.SetActive(false);
        settingsButtons.SetActive(false);

        //Intro
        whiteboardBool = true;
        StartCoroutine(WaitTime(6f));


    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    void ChooseMode()
    {
        anim.SetTrigger("Modes");
        whiteboardButtons.SetActive(false);
        modesBool = true;
        StartCoroutine(WaitTime(3.5f));
    }

    void ChooseModeReturn()
    {
        anim.Play("Modes", -1);
    }

    void Settings()
    {
        anim.SetTrigger("Settings");
        whiteboardButtons.SetActive(false);
        StartCoroutine(WaitTime(6f));
        
        settingsBool = true;
        
        
    }
}
