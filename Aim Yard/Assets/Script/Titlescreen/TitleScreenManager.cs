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

    [SerializeField] private Button optionsReturnButton;
    [SerializeField] private Button audioButton;
    [SerializeField] private Button graphicsButton;
    [SerializeField] private Button resolutionButton;
    [SerializeField] private Button controlsButton;
    
    [Header("Customization Canvas Group")]

    [SerializeField] private CanvasGroup customizationGroup;
    [SerializeField] private Button customizationReturnButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    //Canvas Lerp 
    private float enableStep = 1f;
    private float disableStep = 0f;
    private float maxAplha = 0.9f;
    private float minAlpha = 0.1f;
    private float speed = 5f;
    private float disableTime = 1f;
    private float enableTime = 0f;

    void Start()
    {
        Initialize();

        //Titlescreen Group Canvas Buttons
        if (playButton)
        {
            playButton.onClick.AddListener(delegate { DisableCanvas(titlescreenGroup); EnableCanvas(modesGroup); });
        }

        if (optionsButton)
        {
            optionsButton.onClick.AddListener(delegate { DisableCanvas(titlescreenGroup); EnableCanvas(optionsGroup); });
        }

        if (customizationButton)
        {
            customizationButton.onClick.AddListener(delegate { DisableCanvas(titlescreenGroup); EnableCanvas(customizationGroup); });
        }

        //Options Group Canvas Buttons
        if (optionsReturnButton)
        {
            optionsReturnButton.onClick.AddListener(delegate { EnableCanvas(titlescreenGroup); ; DisableCanvas(optionsGroup);  });
        }

        if (audioButton)
        {
           //audioButton.onClick.AddListener(delegate { DisableCanvas(optionsGroup); EnableCanvas(titlescreenGroup); });
        }

        if(graphicsButton)
        {
            //graphicsButton.onClick.AddListener(delegate { DisableCanvas(optionsGroup); EnableCanvas(titlescreenGroup); });
        }

        if(resolutionButton)
        {
            //resolutionButton.onClick.AddListener(delegate { DisableCanvas(optionsGroup); EnableCanvas(titlescreenGroup); });
        }

        if(controlsButton)
        {
            //controlsButton.onClick.AddListener(delegate { DisableCanvas(optionsGroup); EnableCanvas(titlescreenGroup); });
        }

        //Customization Group Canvas Buttons

        if(customizationReturnButton)
        {
            customizationReturnButton.onClick.AddListener(delegate { DisableCanvas(customizationGroup); EnableCanvas(titlescreenGroup); });
        }


    }
    //Gets current active canvas 
    public void EnableCanvas(CanvasGroup currentCanvas)
    {
        StartCoroutine(LerpDisable(currentCanvas));
    }
    //Gets new active canvas 
    public void DisableCanvas(CanvasGroup newCanvas)
    {
        StartCoroutine(LerpEnable(newCanvas));
    }

    //Lerps canvas group value to 1
    public IEnumerator LerpEnable(CanvasGroup currentCanvas)
    {
        currentCanvas.interactable = false;

        while (currentCanvas.alpha > minAlpha)
        {
            float t = 0;
            t += Time.deltaTime * speed;
            currentCanvas.alpha = Mathf.Lerp(currentCanvas.alpha, minAlpha, t);

            if(currentCanvas.alpha <= minAlpha)
            {
                currentCanvas.alpha = 0;
            }
            yield return null;
        }        
    }
    //Lerps canvas group value to 0
    public IEnumerator LerpDisable(CanvasGroup newCanvas)
    {
        while (newCanvas.alpha < maxAplha)
        {
            float t = 0;
            t += Time.deltaTime * speed;
            newCanvas.alpha = Mathf.Lerp(newCanvas.alpha, maxAplha, t);
            if (newCanvas.alpha >= maxAplha)
            {
                newCanvas.alpha = 1f;
            }
            yield return null;
        }

        newCanvas.interactable = true;

    }

    public void Initialize()
    {
        if(!titlescreenGroup.interactable)
        {
            titlescreenGroup.interactable = true;
        }

        if (!customizationGroup.interactable)
        {
            customizationGroup.interactable = false; ;
        }

        if (!optionsGroup.interactable)
        {
            optionsGroup.interactable = false;
        }

        if (!modesGroup.interactable)
        {
            modesGroup.interactable = false;
        }

        if (titlescreenGroup.alpha <= 0)
        {
            titlescreenGroup.alpha = 1;
        }

        if (customizationGroup.alpha > 0)
        {
            customizationGroup.alpha = 0;
        }

        if (optionsGroup.alpha > 0)
        {
            optionsGroup.alpha = 0;
        }

        if (modesGroup.alpha > 0)
        {
            modesGroup.alpha = 0;
        }
    }
}
