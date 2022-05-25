using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;



public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        GameManager.onGameStateChanged += PlayIntro;
    }

    void OnDestroy()
    {
        GameManager.onGameStateChanged -= PlayIntro;
    }

    private void PlayIntro(GameManager.GameState _state)
    {
        if(!videoPlayer.isPlaying && _state == GameManager.GameState.GAMESTART)
        {
            print("LOAD SCENE");
            int titlescrenSceneIndex = 1;
            GameManager.instance.LoadLevel(titlescrenSceneIndex);
            GameManager.instance.UpdateGameSate(GameManager.GameState.MAINMENU);
        }
    }
}
