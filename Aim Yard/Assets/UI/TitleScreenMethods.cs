using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenMethods : MonoBehaviour
{
    //Beginning UI
    [SerializeField] private Button playButton;
    [SerializeField] private Button QuitButton;

    //Modes Buttons
    [SerializeField] private Button trackingModesButton;
    [SerializeField] private Button shootingModesButton;
    [SerializeField] private Button killHouseModesButton;

    //Modes Clicked
    [SerializeField] private GameObject trackingModes;
    [SerializeField] private GameObject shootingModes;
    [SerializeField] private GameObject killhouseModes;

    //Async loading
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private GameObject loadingIcon;

    void Start()
    {
        //Set UI In-Active
        trackingModesButton.gameObject.SetActive(false);
        shootingModesButton.gameObject.SetActive(false);
        killHouseModesButton.gameObject.SetActive(false);

        //First Active UI
        playButton.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(true);

        if(playButton)
        {
            playButton.onClick.AddListener(PlayButtonClick);
        }

        if(QuitButton)
        {
            QuitButton.onClick.AddListener(QuitGame);
        }

        if(trackingModesButton)
        {
            trackingModesButton.onClick.AddListener(TrackingModesEnable);
        }

        if(shootingModesButton)
        {
            shootingModesButton.onClick.AddListener(ShootingModesEnable);
        }

        if (killHouseModesButton)
        {
            killHouseModesButton.onClick.AddListener(KillhouseModesEnable);
        }


    }

    public void PlayButtonClick()
    {

        //Set Middle Row Active
        trackingModesButton.gameObject.SetActive(true);
        shootingModesButton.gameObject.SetActive(true);
        killHouseModesButton.gameObject.SetActive(true);
    }

    public void SettingsButtonClick()
    {
        //Set Modes In-Active
        shootingModes.SetActive(false);
        trackingModes.SetActive(false);
        killhouseModes.SetActive(false);
        trackingModesButton.gameObject.SetActive(false);
        shootingModesButton.gameObject.SetActive(false);
        killHouseModesButton.gameObject.SetActive(false);
    }

    public void TrackingModesEnable()
    {
        //Disable Shooting + Killhouse
        shootingModes.SetActive(false);
        killhouseModes.SetActive(false);

        //Enable Tracking Modes
        trackingModes.SetActive(true);
    }

    public void ShootingModesEnable()
    {
        //Disable Shooting + Killhouse
        trackingModes.SetActive(false);
        killhouseModes.SetActive(false);

        //Enable Tracking Modes
        shootingModes.SetActive(true);
    }

    public void KillhouseModesEnable()
    {
        //Disable Shooting + Killhouse
        trackingModes.SetActive(false);
        shootingModes.SetActive(false);

        //Enable Tracking Modes
        killhouseModes.SetActive(true);
    }

    public void QuitGame()
    {

    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    //Button Methods
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingIcon.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingSlider.value = progress;

            yield return null;
        }

    }
}
