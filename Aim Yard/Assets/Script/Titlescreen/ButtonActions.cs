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


    private void Start()
    {
        anim = FindObjectOfType<Animator>();

    }

    public void ChooseMode()
    {
        anim.SetTrigger("Gamemode");

    }

    public void ChangeSettings()
    {
        anim.SetTrigger("Settings");
    }

    public void EndGame()
    {
   
    }
}
