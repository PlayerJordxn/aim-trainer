using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridshotManager : MonoBehaviour
{
    public static GridshotManager instance;
    PlayerController playerControllerScript;

    [SerializeField] private GameObject characterM4;
    [SerializeField] private GameObject characterGlock;

    private Vector3 spawnLocation;
    Quaternion spawnRotation;

    private float countdown = 5f;
    public int currentTargetCount = 0;
    [SerializeField] private int maxTargetCount = 5;

    public GameState gameState;

    public enum GameState
    {
        ROUNDSTART,
        COUNTDOWN,
        PLAYING,
        ROUNDEND,
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        spawnRotation = Quaternion.Euler(0, 180, 0);
        spawnLocation = GameObject.FindGameObjectWithTag("SpawnLocation").transform.position;

        LoadCharacter(PlayerPrefs.GetString("WeaponSelection"));

        gameState = GameState.ROUNDSTART;
    }

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
        if (selectedWeapon == "M4")
        {
            characterM4.SetActive(true);
            characterM4.transform.position = spawnLocation;
            characterM4.transform.rotation = spawnRotation; 
        }
        else//Glock
        {
            characterGlock.SetActive(true);
            characterGlock.transform.position = spawnLocation;
            characterGlock.transform.rotation = spawnRotation;
        }
    }

    private void ShootToStart()
    {
        bool shootToStartInput = Input.GetKeyDown(KeyCode.Mouse0);
        if(shootToStartInput)
        {
            gameState = GameState.COUNTDOWN;
        }
    }

    public void Countdown()
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
            countdown -= Time.deltaTime;
        }
    }

    public void GameActive()
    {
        if(currentTargetCount < maxTargetCount)
        {
            print("TARGET SPAWNED");
            currentTargetCount++;
            ObjectPool.instance.GetTarget();
        }
    }
}
