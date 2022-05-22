using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitlescreenManager : MonoBehaviour
{
    [Header("Titlescreen Canvas Group")]
    [SerializeField] private CanvasGroup titlescreenGroup;
    //Titlescreen buttons
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button customizationButton;

    [Header("Modes Canvas Group")]
    [SerializeField] private CanvasGroup modesGroup;
    [SerializeField] private Button gridshotButton;
    [SerializeField] private Button colourCordinationButton;
    [SerializeField] private Button headshotModeButton;
    [SerializeField] private Button movingTargetButton;
    [SerializeField] private Button scaleModeButton;
    [SerializeField] private Button colourCordinationTrackingButton;
    [SerializeField] private Button scaleTrackingButton;

    [Header("Options Canvas Group")]
    [SerializeField] private CanvasGroup optionsGroup;

    [Header("Customization Canvas Group")]

    [SerializeField] private CanvasGroup customizationGroup;

        //Canvas Lerp 
    private float enableStep = 1f;
    private float disableStep = 0f;
    private float maxAplha = 1f;
    private float minAlpha = 0f;
    private float speed = 5f;
    private float disableTime = 1f;
    private float enableTime = 0f;

    void Start()
    {
        //Titlescreen Buttons
        if (playButton)
            playButton.onClick.AddListener(delegate { DisableCanvas(titlescreenGroup); EnableCanvas(modesGroup); });

        if (optionsButton)
            optionsButton.onClick.AddListener(delegate { DisableCanvas(titlescreenGroup); EnableCanvas(optionsGroup); });

        if (customizationButton)
            customizationButton.onClick.AddListener(delegate { DisableCanvas(titlescreenGroup); EnableCanvas(customizationGroup); });
        

        //Modes Canvas


        //
    }

    public void EnableCanvas(CanvasGroup currentCanvas)
    {
        StartCoroutine(LerpDisable(currentCanvas));
    }

    public void DisableCanvas(CanvasGroup newCanvas)
    {
        StartCoroutine(LerpEnable(newCanvas));
    }

    public IEnumerator LerpEnable(CanvasGroup currentCanvas)
    {
        while(currentCanvas.alpha > minAlpha)
        {
            currentCanvas.alpha = Mathf.Lerp(currentCanvas.alpha, minAlpha, Time.deltaTime * 5);
            yield return null;
        }
        print("DONE");
        
    }

    public IEnumerator LerpDisable(CanvasGroup newCanvas)
    {
        while (newCanvas.alpha < maxAplha)
        {
            newCanvas.alpha = Mathf.Lerp(newCanvas.alpha, maxAplha, Time.deltaTime * 5);
            yield return null;
        }
    }
}
