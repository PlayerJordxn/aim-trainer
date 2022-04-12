using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ScaleTrackingManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera[] cams;//0 = M4A1; 1 = M16; 2 = Glock;
    [SerializeField] private Camera mainCamera;

    [Header("Characters")]
    [SerializeField] private GameObject[] playerCharacters = new GameObject[3]; //0 = M4A1; 1 = M16; 2 = Glock;
    public PlayerController playerController;

    [Header("Game Settings")]
    private int targetCount;
    private int currentScore = 0;
    private int timer = 0;
    private bool isPlaying = false;
    private bool roundEnd;
    private float shootTimeElapsed = 0f;

    [Header("Score")]
    private int scoreBonus = 0;
    private int targetScoreValue = 10;
    private bool decrementing = false;
    private float currentTargetCount = 0;

    [Header("Unity Action")]
    private Action onCountdownBegin;
    private Action onRoundEnd;

    [Header("Audio")]
    //Countdown 
    [SerializeField] private AudioSource BeepSoundSource;
    //Wind
    [SerializeField] private AudioSource windAudioSource;
    //Target Hit
    [SerializeField] private AudioSource targetHitAudioSource;
    [SerializeField] private AudioClip targetHitAudioClip;

    [Header("User Interface")]
    [SerializeField] private GameObject crosshair;
    //Score UI
    [SerializeField] private GameObject roundResultsParent;
    [SerializeField] private Button exitRoundResultsButton;

    [SerializeField] private GameObject scoreUI;

    [SerializeField] private TextMeshProUGUI totalTargetCountText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private TextMeshProUGUI finalTargetCount;
    [SerializeField] private TextMeshProUGUI finalScore;


    [Header("Countdown Variables")]
    [SerializeField] private RectTransform[] countdownCircles = new RectTransform[3];
    [SerializeField] private TextMeshProUGUI shootToStartText;
    [SerializeField] private TextMeshProUGUI countdownText;
    private int countdown = 5;
    private bool countdownStartInitiated = false;
    [SerializeField] private GameObject countdownParent;



    private void Awake()
    {
        //PlayerPrefs.GetInt("Character")
        LoadCharacter(2);

    }

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerController = FindObjectOfType<PlayerController>();
        roundEnd = false;

        if (exitRoundResultsButton)
        {
            exitRoundResultsButton.onClick.AddListener(delegate { DisableRoundResults(); });
        }
    }

    // Update is called once per frame
    void Update()
    {
        ScoreboardText(timer, currentScore);      //Scorboard text
        ShootInput();                                       //Shoot
        EventListeners();
        //Update countdown text
        countdownText.text = countdownStartInitiated ? countdownText.text = countdown.ToString() : countdownText.text = "5";

        if (IsPlaying())
        {
            shootToStartText.gameObject.SetActive(false);   //Enable shoot to start text
            scoreUI.SetActive(true);                        //Enable score UI
            crosshair.SetActive(true);                      //Enable crosshair
            windAudioSource.UnPause();                      //Play wind audio
            countdownStartInitiated = false;                //Determins if countdown start
            if (!decrementing)
            {
                StartCoroutine(GameTimer(1));//Game timer
            }

            if (targetCount < 1)
            {
                ColourCordinationTrackingPool.instance.GetTarget();    //Get target
                targetCount++;                      //Increment target count
            }
        }
        else
        {
            if (roundEnd)
            {
                onRoundEnd += RoundEnd;
                roundEnd = false;
            }

            if (!countdownStartInitiated)
            {
                countdown = 5;                              //Reset countdown
            }
            countdownText.text = countdown.ToString();      //Update countdown text
            scoreUI.SetActive(false);                       //Disable in game score UI
            windAudioSource.Pause();                        //Pause wind audio
            crosshair.SetActive(false);                     //Disable crosshair
            EnableCountdown();
        }
    }

    public void EventListeners()
    {
        if (onCountdownBegin != null)                       //Check for listener
        {
            onCountdownBegin.Invoke();
        }

        if (onRoundEnd != null)
        {
            onRoundEnd.Invoke();
        }
    }

    public void DisableRoundResults()
    {
        finalScore.text = "Final Score: " + currentScore.ToString();
        roundEnd = false;
        onRoundEnd -= RoundEnd;
        roundResultsParent.SetActive(false);
        currentScore = 0;
        shootToStartText.gameObject.SetActive(true);    //Enable text
    }

    private void ShootInput()
    {
        if (IsPlaying())
        {
            float shootDelay = 0.045f;
            if (shootTimeElapsed > shootDelay)
            {
                shootTimeElapsed = 0f;
                Shoot();
            }
            else
            {
                shootTimeElapsed += Time.deltaTime;
            }
        }
    }

    private void EnableCountdown()
    {
        //Input
        bool enableGameInput = Input.GetKeyDown(KeyCode.Mouse0);
        if (enableGameInput && !countdownStartInitiated && !roundResultsParent.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            countdownStartInitiated = true;
            countdownParent.SetActive(true);
            countdownText.gameObject.SetActive(true);
            onCountdownBegin += RotateCircles;
            StartCoroutine(StartCountdown(1f));
        }
    }

    IEnumerator StartCountdown(float _wait)
    {
        onCountdownBegin += RotateCircles;
        BeepSoundSource.Play();
        yield return new WaitForSecondsRealtime(_wait);
        countdown--;
        BeepSoundSource.Play();
        yield return new WaitForSecondsRealtime(_wait);
        countdown--;
        BeepSoundSource.Play();
        yield return new WaitForSecondsRealtime(_wait);
        countdown--;
        BeepSoundSource.Play();
        yield return new WaitForSecondsRealtime(_wait);
        countdown--;
        BeepSoundSource.Play();
        yield return new WaitForSecondsRealtime(_wait);
        countdown--;
        BeepSoundSource.Play();

        countdownParent.SetActive(false);               //Disable countdown
        scoreUI.gameObject.SetActive(true);             //Enable score UI
        timer = 60;                                     //Set timer at start
        onCountdownBegin -= RotateCircles;              //Remove listener
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void RotateCircles()
    {
        float speed = 10f;
        countdownCircles[0].transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
        countdownCircles[1].transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
        countdownCircles[2].transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));

    }
    private void Shoot()
    {
        RaycastHit hit;
        float rayDistance = 100f;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward), out hit, rayDistance))
        {
            if (hit.collider.CompareTag("TrackingTarget"))
            {


                targetHitAudioSource.PlayOneShot(targetHitAudioClip);

                TargetHealth targetScript = hit.collider.GetComponent<TargetHealth>();

                AddScore();

                if (targetScript.currentHealth > 0)
                {
                    //Reduce health
                    float healthReduction = 10f;
                    targetScript.currentHealth -= healthReduction;

                }
                else
                {
                    //Return to pool
                    ColourCordinationTrackingPool.instance.ReturnTarget(hit.collider.gameObject);
                    targetCount--;
                    //Target destroyed 
                    currentTargetCount++;
                }

            }
            else
            {
                //Reset multiplier
                scoreBonus = 0;
            }
        }
    }

    public void AddScore()
    {
        //Score tracker
        currentScore += targetScoreValue + scoreBonus;
        //Increase multiplier
        if (scoreBonus < 100)
        {
            scoreBonus += 10;
        }
    }

    public void RoundEnd()
    {
        roundResultsParent.SetActive(true);
        finalTargetCount.text = "Final Target Count: " + currentTargetCount.ToString();
        finalScore.text = "Final Score: " + currentScore.ToString();

        GameObject[] activeTargets;
        activeTargets = GameObject.FindGameObjectsWithTag("TrackingTarget");

        for (int i = 0; i < activeTargets.Length; i++)
        {
            activeTargets[i].SetActive(false);
            ColourCordinationTrackingPool.instance.ReturnTarget(activeTargets[i]);
            targetCount--;
        }
        Cursor.lockState = CursorLockMode.Confined;
    }



    private bool IsPlaying()
    {
        if (timer > 0)
        {
            return isPlaying = true;
        }
        else
        {
            return isPlaying = false;
        }
    }

    private void LoadCharacter(int _data)
    {
        if (_data == 0)
        {
            //Load M4A1 Data
            playerCharacters[_data].SetActive(true);
            mainCamera = cams[_data];
        }
        else if (_data == 1)
        {
            //Load M16 Data
            playerCharacters[_data].SetActive(true);
            mainCamera = cams[_data];
        }
        else
        {
            //Load Glock Data
            playerCharacters[_data].SetActive(true);
            mainCamera = cams[_data];
        }
    }

    private void ScoreboardText(int _timer, int _score)
    {
        //Set timer text
        timerText.text = _timer > 0 ? timerText.text = _timer.ToString() : timerText.text = "0";
        //Set Score Text
        scoreText.text = _score > 0 ? scoreText.text = _score.ToString() : scoreText.text = "0";

        totalTargetCountText.text = _score > 0 ? totalTargetCountText.text = currentTargetCount.ToString() : totalTargetCountText.text = "0";
    }

    private IEnumerator GameTimer(int _wait)
    {
        decrementing = true;
        yield return new WaitForSecondsRealtime(_wait);
        timer--;
        if (timer == 1)
        {
            roundEnd = true;
        }
        decrementing = false;
    }

    private void SaveRoundReslut()
    {

    }
}
