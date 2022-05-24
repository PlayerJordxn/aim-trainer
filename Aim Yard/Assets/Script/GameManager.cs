using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class GameManager : MonoBehaviour
{
    //Instance
    public static GameManager instance { get; private set; }

    [Header("Loading Variables")]
    private float loadingScreenTime = 5f;

    [Header("Controller")]
    public PlayerControls playerController;

    [Header("Input Actions")]
    private InputAction pauseInput;

    public enum GameState
    {
        TITLESCREEN,
        PLAYING,
        PAUSED,
    }
    public GameState CurrentGameState;

    private void Awake()
    {
        playerController = new PlayerControls();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance != null)
        {
            Destroy(this);
        } 
    }

    void Update()
    {
        switch(CurrentGameState)
        {
            case GameState.TITLESCREEN:

                break;

            case GameState.PLAYING:

                PauseGameInput(CurrentGameState);

                break;

            case GameState.PAUSED:

                
                PauseGameInput(CurrentGameState);
                EnablePauseMenu(); 

                break;

            default:
                Debug.LogError("Game state is set to default case");
                break;
        }
    }

    private void EnablePauseMenu()
    {
        throw new NotImplementedException();
    }

    public void PauseGameInput(GameState _currentState)
    {
        bool pauseInput = playerController.UI.PauseInput.triggered;
        if (pauseInput)
        {
            //Switches between pause and unpaused
            CurrentGameState = _currentState == GameState.PAUSED ? GameState.PLAYING : GameState.PAUSED;
        }
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevel(int _sceneIndex)
    {
        StartCoroutine(LoadingScene(loadingScreenTime, _sceneIndex));
    }

    public IEnumerator LoadingScene(float _waitTime, int _sceneIndex)
    {
        //Wait 5 seconds
        WaitForSecondsRealtime realTimeSeconds = new WaitForSecondsRealtime(_waitTime);
        yield return realTimeSeconds;

        //Load scene
        SceneManager.LoadScene(_sceneIndex);
    }
        
}

