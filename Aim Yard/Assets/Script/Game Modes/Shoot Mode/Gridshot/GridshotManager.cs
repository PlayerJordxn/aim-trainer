using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridshotManager : MonoBehaviour
{
    public GameObject test;
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.onGameStateChanged += Test; 
    }

    void Start()
    {
        print("Game Start");
    }

    void OnDestroy()
    {
        GameManager.onGameStateChanged -= Test;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Test(GameManager.GameState _state)
    {
        test.SetActive(_state == GameManager.GameState.LOADING);
    }
}
