using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitlescreenManager : MonoBehaviour
{
    //Menu Parents
    [Header("Menu Parents")]
    public GameObject mainMenuParent;
    public GameObject levelSelectionParent;
    public GameObject optionsParent;

    [Header("Main Menu")]
    public Button playButton;
    public Button optionsButton;
    public Button customizationButton;

    [Header("Play Menu")]
    public Button modesReturnButton;
    public Button trackingModesButton;
    public Button shootingModesButton;
    public Button specialModesButton;

    [Header("Tracking Modes Levels")]
    public Button singleTargetTrack;
    public Button precisionTrackingButton;
    public Button colourTrackingButton;
    public Button scaleTrackingButton;
    public Button offsetTrackingMode;

    [Header("Shooting Modes Buttons")]
    public Button gridshotModeButton;
    public Button precisionModeButton;
    public Button flickshotModeButton;
    public Button movingTargetsModeButton;
    public Button colourCordinationModeButton;

    [Header("Special Modes Buttons")]
    public Button killhouseButton;
    public Button newLevelButton;
    [Header("Options")]
    public Button template;

    //Customization
    [Header("Cusomization")]
    public Button weaponLeftButton;
    public Button weaponRightButton;

    [Header("Shooting Modes")]
    public GameObject customizationParent;
    public GameObject trackingModesParent;
    public GameObject shootingModesParent;
    public GameObject specialModesParent;
    private void Awake()
    {
        GameManager.onGameStateChanged += MainMenu;
        GameManager.onGameStateChanged += LevelSelection;
        GameManager.onGameStateChanged += Options;
        GameManager.onGameStateChanged += Customization;
        GameManager.onGameStateChanged += ShootingModes;
        GameManager.onGameStateChanged += TrackingModes;
        GameManager.onGameStateChanged += SpecialModes;
    }

    void OnDestroy()
    {
        GameManager.onGameStateChanged -= MainMenu;
        GameManager.onGameStateChanged -= LevelSelection;
        GameManager.onGameStateChanged -= Options;
        GameManager.onGameStateChanged -= Customization;
        GameManager.onGameStateChanged -= ShootingModes;
        GameManager.onGameStateChanged -= TrackingModes;
        GameManager.onGameStateChanged -= SpecialModes;
    }


    void Start()
    {
        //Main Menu Button Listeners
        if(playButton) playButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.LEVELSELECTION); });
        if(optionsButton) optionsButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.OPTIONS); });
        if(customizationButton) customizationButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.CUSTOMIZATION); });

        //Play Menu
        if (trackingModesButton) trackingModesButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.TRACKINGMODES); });
        if (shootingModesButton) shootingModesButton.onC lick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.SHOOTINGMODES); });
        if (specialModesButton) specialModesButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.SPECIALMODES); });

        //Tracking Modes Buttons
        if (precisionTrackingButton) precisionTrackingButton.onClick.AddListener(delegate { GameManager.instance.LoadLevel(0); });
        if (singleTargetTrack) singleTargetTrack.onClick.AddListener(delegate { GameManager.instance.LoadLevel(0); });
        if (colourTrackingButton) colourTrackingButton.onClick.AddListener(delegate { GameManager.instance.LoadLevel(0); });
        if (offsetTrackingMode) offsetTrackingMode.onClick.AddListener(delegate { GameManager.instance.LoadLevel(0); });
        if (scaleTrackingButton) scaleTrackingButton.onClick.AddListener(delegate { GameManager.instance.LoadLevel(0); });

        //Shooting Modes Buttons

        //Special Modes Buttons

    }

    private void MainMenu(GameManager.GameState state)
    {
        mainMenuParent.SetActive(state == GameManager.GameState.MAINMENU);
    }
    private void LevelSelection(GameManager.GameState state)
    {
        levelSelectionParent.SetActive(state == GameManager.GameState.LEVELSELECTION);
    }
    private void Options(GameManager.GameState state)
    {
        optionsParent.SetActive(state == GameManager.GameState.OPTIONS);
    }
    private void Customization(GameManager.GameState state)
    {
        customizationParent.SetActive(state == GameManager.GameState.CUSTOMIZATION);
    }
    private void ShootingModes(GameManager.GameState state)
    {
        shootingModesParent.SetActive(state == GameManager.GameState.SHOOTINGMODES);
    }
    private void TrackingModes(GameManager.GameState state)
    {
        trackingModesParent.SetActive(state == GameManager.GameState.TRACKINGMODES);
    }
    private void SpecialModes(GameManager.GameState state)
    {
        specialModesParent.SetActive(state == GameManager.GameState.SPECIALMODES);
    }

}
