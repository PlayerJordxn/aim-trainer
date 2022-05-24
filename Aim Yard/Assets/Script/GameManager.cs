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

    [Header("Loading Variables")]
    private float loadingScreenTime = 5f;

    [Header("Controller")]
    public PlayerControls playerController;

    [Header("Pause Menu")]
    private GameObject pauseMenuCanvas;

    [Header("Input Actions")]
    private InputAction pauseInput;

    public static event Action<GameState> onGameStateChanged;

    public enum GameState
    {
        GAMESTART,  //This state is when the game is first launched
        MAINMENU,//Titlescreen state
        LEVELSELECTION,
        OPTIONS,
        CUSTOMIZATION,
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
            Destroy(this);
        } 
    }

    void Start()
    {
        UpdateGameSate(GameState.GAMESTART);
    }

    public void UpdateGameSate(GameState _newState)
    {
        print(CurrentGameState);
        CurrentGameState = _newState;
        switch (CurrentGameState)
        {
            case GameState.GAMESTART:
                break;
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

