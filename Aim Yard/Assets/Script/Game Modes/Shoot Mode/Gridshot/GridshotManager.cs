using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridshotManager : MonoBehaviour
{
    public PlayerControls playerController;


    [Header("Round start variables")]
    float roundStartCountdown = 5f;

    [Header("Countdown variables")]
    private bool countdownActive = false;

    public GameState gameState;

    // Start is called before the first frame update
    private void Awake()
    {
        playerController = new PlayerControls();
        playerController.Enable();
    }

    private void Start()
    {
        LoadCharacter(PlayerPrefs.GetString("WeaponSelection"));
        gameState = GameState.ROUNDSTART;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (gameState)
        {
            case GameState.ROUNDSTART:
                ShootToStart();
                break;

            case GameState.COUNTDOWN:
                Countdown();
                break;

            case GameState.PLAYING:
                GameActive();
                break;

            case GameState.ROUNDEND:

                break;

            
        }
    }

    private void LoadCharacter(string selectedWeapon)
    {
        if(selectedWeapon == "M4")
        {

        }
        else if(selectedWeapon == "M16")
        {

        }
        else//Glock
        {

        }
    }

    private void ShootToStart()
    {
        bool startRoundInput = playerController.UI.ShootToStartInput.triggered;
        if (startRoundInput)
        {
            //Start Countdown State
            gameState = GameState.COUNTDOWN;
        }
    }

    public void Countdown()
    {
        float minTime = 0f;
        roundStartCountdown -= Time.deltaTime; 
        if(roundStartCountdown < minTime)
        {
            gameState = GameState.PLAYING;
        }
    }
    private IEnumerator CountDown()
    {
        while(roundStartCountdown > 0)
        {
            yield return null;
            roundStartCountdown -= Time.deltaTime;
        }

        gameState = GameState.PLAYING;
    }

    public void GameActive()
    {

    }

    public enum GameState
    {
        ROUNDSTART,
        COUNTDOWN,
        PLAYING,
        ROUNDEND,
    }
}
