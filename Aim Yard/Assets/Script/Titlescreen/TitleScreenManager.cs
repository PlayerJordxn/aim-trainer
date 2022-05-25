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
    public Button shootingModesButton;
    public Button trackingModesButton;
    public Button specialModesButton;
    public Button shootingModesReturnButton;
    public Button trackingModesReturnButton;
    public Button specialModesReturnButton;

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

        //Modes Buttons
        if (shootingModesButton) shootingModesButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.SHOOTINGMODES); });
        if (trackingModesButton) trackingModesButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.TRACKINGMODES); });
        if (specialModesButton) specialModesButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.SPECIALMODES); });

        if (shootingModesButton) shootingModesButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.LEVELSELECTION); });
        if (trackingModesButton) trackingModesButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.LEVELSELECTION); });
        if (specialModesButton) specialModesButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.LEVELSELECTION); });
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
