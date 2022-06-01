using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    //Instance
    public static GameManager instance { get; private set; }

    [Header("Controller")]

    [Header("Pause Menu")]
    private GameObject pauseMenuCanvas;


    public static event Action<GameState> onGameStateChanged;

    public enum GameState
    {
        MAINMENU,//Titlescreen state
        LEVELSELECTION,
        OPTIONS,
        CUSTOMIZATION,
        LOADING,
        ROUNDSTART,
        PLAYING,    //in active game scene
        ROUNDEND,
        PAUSED,
    }
    public GameState CurrentGameState;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        } 
    }

    void Start()
    {
        UpdateGameSate(GameState.MAINMENU);
    }

    //Changes the game state
    public void UpdateGameSate(GameState _newState)
    {
        CurrentGameState = _newState;
        switch (CurrentGameState)
        {
            case GameState.MAINMENU:
                break;
            case GameState.ROUNDSTART:
                break;
            case GameState.PLAYING:
                break;
            case GameState.ROUNDEND:
                break;
            case GameState.PAUSED:
                break;
            case GameState.LEVELSELECTION:
                break;
            case GameState.OPTIONS:
                break;
            case GameState.CUSTOMIZATION:
                break;
            case GameState.LOADING:
                break;
            default:
                Debug.LogError("Game state is set to default case");
                break;
        }

        onGameStateChanged?.Invoke(_newState);
    }

    public void PauseGameInput(GameState _currentState)
    {

    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadLevel(int _loadIndex)
    {
        SceneManager.LoadScene(_loadIndex, LoadSceneMode.Single);
        UpdateGameSate(GameState.LOADING);
    }

}

