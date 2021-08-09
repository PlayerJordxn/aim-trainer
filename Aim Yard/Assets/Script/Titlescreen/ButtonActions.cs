using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    Animator anim;

    //Game Modes
    [SerializeField] GameObject colorCordinationButton;
    [SerializeField] GameObject flickShotButton;
    [SerializeField] GameObject gridshotButton;
    [SerializeField] GameObject headshotModeButton;
    [SerializeField] GameObject rangeTrainingButton;
    [SerializeField] GameObject colorCordinationTracking;
    [SerializeField] GameObject singleTargetTracking;

    //Booleans
    bool settings = false;
    bool gamemodes = false;

    private void Start()
    {
        anim = FindObjectOfType<Animator>();
    }

    public void ChooseMode()
    {
        Debug.Log("Mode Chosen");
        //anim.SetTrigger("Modes");

    }

    public void ChangeSettings()
    { 

        Debug.Log("Settings Chosen");
        //anim.SetTrigger("Settings");
    }

    public void EndGame()
    {
        Debug.Log("End Game Chosen");
    }
}
