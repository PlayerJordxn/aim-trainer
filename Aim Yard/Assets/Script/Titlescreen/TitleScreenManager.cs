using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitlescreenManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingSlider;

    [SerializeField] private CanvasGroup titlescreenGroup;
    [SerializeField] private CanvasGroup modesGroup;
    [SerializeField] private CanvasGroup optionsGroup;
    [SerializeField] private CanvasGroup customizationGroup;

    //Titlescreen buttons
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button customizationButton;

    //Load scene buttons
    //Shooting Modes Buttons
    [SerializeField] private Button gridshotButton;
    [SerializeField] private Button colourCordinationButton;
    [SerializeField] private Button headshotModeButton;
    [SerializeField] private Button movingTargetButton;
    [SerializeField] private Button scaleModeButton;

    //Tracking modes
    [SerializeField] private Button colourCordinationTrackingButton;
    [SerializeField] private Button scaleTrackingButton;



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
        {
            playButton.onClick.AddListener(delegate { StartCoroutine(SwitchCanvas(titlescreenGroup, modesGroup)); });
        }

        if (optionsButton)
        {
            optionsButton.onClick.AddListener(delegate { StartCoroutine(SwitchCanvas(titlescreenGroup, optionsGroup)); });
        }

        if (customizationButton)
        {
            customizationButton.onClick.AddListener(delegate { StartCoroutine(SwitchCanvas(titlescreenGroup, customizationGroup)); });
        }

        //Modes buttons
    }

    public IEnumerator SwitchCanvas(CanvasGroup currentCanvas, CanvasGroup newCanvas)
    {
        while(currentCanvas.alpha > minAlpha)
        {
            currentCanvas.alpha = Mathf.Lerp(currentCanvas.alpha, minAlpha, Time.time);
            yield return null;
        }

        while (newCanvas.alpha < maxAplha)
        {
            newCanvas.alpha = Mathf.Lerp(newCanvas.alpha, maxAplha, Time.time);
            yield return null;
        }
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    private IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;
            yield return null;
        }
    }
}
