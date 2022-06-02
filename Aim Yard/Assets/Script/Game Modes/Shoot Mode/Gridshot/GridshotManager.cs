using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridshotManager : MonoBehaviour
{
    [Header("Game Settings")]
    public static GridshotManager instance;
    PlayerController playerControllerScript;

    [SerializeField] private GameObject characterM4;
    [SerializeField] private GameObject characterGlock;

    private float countdown = 5f;
    public float countdownRotationSpeed = 10f;
    public int currentTargetCount = 0;
    private bool isDecrementing = false;
    [SerializeField] private int maxTargetCount = 5;
    public GameState gameState;

    public enum GameState
    {
        ROUNDSTART,
        COUNTDOWN,
        PLAYING,
        ROUNDEND,
    }

    [Header("UI")]
    [SerializeField] private GameObject shootToStartText;
    [SerializeField] private GameObject resultsScreen;
    [SerializeField] private GameObject crosshair;

    [Header("Countdown UI")]
    [SerializeField] private GameObject countdownParent;
    [SerializeField] private RectTransform circleOne;
    [SerializeField] private RectTransform circleTwo;
    [SerializeField] private TextMeshProUGUI countdownText;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LoadCharacter(PlayerPrefs.GetString("WeaponSelection"));
        gameState = GameState.ROUNDSTART;
    }

    private void Update()
    {
        //UI enables during specific game state
        shootToStartText.SetActive(gameState == GameState.ROUNDSTART);
        resultsScreen.SetActive(gameState == GameState.ROUNDEND);
        crosshair.SetActive(gameState == GameState.PLAYING);
        countdownParent.SetActive(gameState == GameState.COUNTDOWN);

        //Game state
        switch (gameState)
        {
            case GameState.ROUNDSTART:

                ShootToContinue(GameState.COUNTDOWN);
                CanvasManager.instance.ResetScoreboard();

                break;

            case GameState.COUNTDOWN:

                StartCountdown();
                CountdownRotation();
               
                break;

            case GameState.PLAYING:

                GameActive();
                if (!isDecrementing) StartCoroutine(GameTimer(1f));

                break;

            case GameState.ROUNDEND:

                ShootToContinue(GameState.ROUNDSTART);

                break;
        }
    }

    private void CountdownRotation()
    {
        //rotate countdown ui
        Vector3 newRoation = Vector3.forward * Time.deltaTime * countdownRotationSpeed;
        circleOne.Rotate(newRoation / 2);
        circleTwo.Rotate(newRoation);
        countdownText.text = countdown.ToString();
    }

    private IEnumerator GameTimer(float wait)
    {

        isDecrementing = true;
        WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(wait);
        yield return waitTime;
        CanvasManager.instance.time--;
        isDecrementing = false;
    }

    public void ReturnTargets()
    {

    }

    private void LoadCharacter(string selectedWeapon)
    {
        if (selectedWeapon == "M4") characterM4.SetActive(true);
        else characterGlock.SetActive(true);
    }

    private void ShootToContinue(GameState newState)
    {
        bool shootToStartInput = Input.GetKeyDown(KeyCode.Mouse0);
        if(shootToStartInput)
        {
            if(newState == GameState.ROUNDEND)
            {
                ReturnTargets();
            }

            gameState = newState;
        }
    }

    public void StartCountdown()
    {
        float minTime = 0f;
        float resetValue = 5f;

        if (countdown <= minTime)
        {
            countdown = resetValue;
            gameState = GameState.PLAYING;
        }
        else
        {
            if(!isDecrementing) StartCoroutine(Countdown(1f));
        }
    }

    private IEnumerator Countdown(float wait)
    {
        print(countdown);
        isDecrementing = true;
        yield return new WaitForSecondsRealtime(wait);
        countdown--;
        isDecrementing = false;
    }

    public void GameActive()
    {
        if(currentTargetCount < maxTargetCount)
        {
            currentTargetCount++;
            ObjectPool.instance.GetTarget();
        }

        if(CanvasManager.instance.time <= 1)
        {
            gameState = GameState.ROUNDEND;
        }

    }
}
