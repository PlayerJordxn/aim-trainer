using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitlescreenManager : MonoBehaviour
{
    public GameObject mainMenuParent;
    public GameObject levelSelectionParent;
    public GameObject optionsParent;
    public GameObject customizationParent;

    [Header("Main Menu")]
    public Button playButton;
    public Button optionsButton;
    public Button customizationButton;

    [Header("Options")]
    public Button template;

    //Customization
    [Header("Cusomization")]
    public Button weaponLeftButton;
    public Button weaponRightButton;


    private void Awake()
    {
        GameManager.onGameStateChanged += MainMenu;
        GameManager.onGameStateChanged += LevelSelection;
        GameManager.onGameStateChanged += Options;
        GameManager.onGameStateChanged += Customization;
    }

    void Start()
    {
        //Main Menu Button Listeners
        if(playButton) playButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.LEVELSELECTION); });
        if(optionsButton) optionsButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.OPTIONS); });
        if(customizationButton) customizationButton.onClick.AddListener(delegate { GameManager.instance.UpdateGameSate(GameManager.GameState.CUSTOMIZATION); });

    }
    void OnDestroy()
    {
        GameManager.onGameStateChanged -= MainMenu;
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

}
