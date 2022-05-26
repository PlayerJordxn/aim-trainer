using System;
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

    [Header("Controller")]
    public PlayerControls playerController;

    [Header("Pause Menu")]
    private GameObject pauseMenuCanvas;

    [Header("Input Actions")]
    private InputAction pauseInput;

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
        playerController = new PlayerControls();

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
        print(_newState);
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
        bool pauseInput = playerController.UI.PauseInput.triggered;
        if (pauseInput)
        {
            
        }
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadLevel(int _loadIndex)
    {
        print("Loading Scene...");
        print(_loadIndex);

        SceneManager.LoadScene(_loadIndex, LoadSceneMode.Single);
        //UpdateGameSate(GameState.LOADING);
    }

}

