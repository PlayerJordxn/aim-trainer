using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlescreenManager : MonoBehaviour
{
    [Header("Menu Parents")]
    //Main Menu
    [SerializeField] private GameObject mainMenuParent;
    //Level Selection
    [SerializeField] private GameObject levelSelectionParent;
    //Options
    [SerializeField] private GameObject optionsParent;
    //Customization
    [SerializeField] private GameObject customizationParent;

    [Header("Main Menu")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button customizationButton;

    [Header("Mode Selection Menu")]
    [SerializeField] private Button levelsReturnButton;

    [Header("Cusomization Menu")]
    [SerializeField] private Button weaponLeftButton;
    [SerializeField] private Button weaponRightButton;


    [Header("Level Load Buttons")]
    //Tracking    
    [SerializeField] private Button singleTargetTracking;
    [SerializeField] private Button precisionTrackingButton;
    [SerializeField] private Button colourCourdinationTrackingButton;
    [SerializeField] private Button scaleTrackingButton;
    [SerializeField] private Button offsetTrackingMode;

    [SerializeField] private Button gridshotModeButton;
    [SerializeField] private Button precisionModeButton;
    [SerializeField] private Button flickshotModeButton;
    [SerializeField] private Button movingTargetsModeButton;
    [SerializeField] private Button colourCordinationModeButton;

    [SerializeField] private Button killhouseButton;
    [SerializeField] private Button newLevelButton;
    private void Awake()
    {
        GameManager.onGameStateChanged += MainMenu;
    }

    void OnDestroy()
    {
        GameManager.onGameStateChanged -= MainMenu;
    }


    void Start()
    {
        if (gridshotModeButton)
        {
            gridshotModeButton.onClick.AddListener(delegate { GameManager.instance.LoadLevel(1); });
        }

        //Main Menu Button Listeners
        if (playButton) playButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.LEVELSELECTION); });
        if (optionsButton) optionsButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.OPTIONS); });
        if (customizationButton) customizationButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.CUSTOMIZATION); });

        //Levels
        if (levelsReturnButton) levelsReturnButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.MAINMENU); });

    }

    private void MainMenu(GameManager.GameState state)
    {
        mainMenuParent.SetActive(state == GameManager.GameState.MAINMENU);
        levelSelectionParent.SetActive(state == GameManager.GameState.LEVELSELECTION);
        optionsParent.SetActive(state == GameManager.GameState.OPTIONS);
        customizationParent.SetActive(state == GameManager.GameState.CUSTOMIZATION);

    }
    public int ReturnLevelIndex(int _index)
    {
        return _index;
    }
}
