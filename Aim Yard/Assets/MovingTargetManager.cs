using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class MovingTargetManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera[] cams;//0 = M4A1; 1 = M16; 2 = Glock;
    [SerializeField] private Camera mainCamera;

    [Header("Characters")]
    [SerializeField] private GameObject[] playerCharacters = new GameObject[3]; //0 = M4A1; 1 = M16; 2 = Glock;
    public PlayerController playerController;

    [Header("Game Settings")]
    private int targetCount;
    private float accuracy = 0f;
    private int currentScore = 0;
    private int timer = 0;
    private bool isPlaying = false;
    private bool roundEnd;

    [Header("Score")]
    private float previousScore = 0f;
    private int scoreBonus = 0;
    private int targetScoreValue = 100;
    private bool decrementing = false;
    private int totalTargetsHit = 0;
    private int totalShotsFired = 0;
    private float currentAccuracy;

    [Header("Unity Action")]
    private Action onCountdownBegin;
    private Action onRoundEnd;

    [Header("Audio")]
    //Countdown 
    [SerializeField] private AudioSource BeepSoundSource;
    //Wind
    [SerializeField] private AudioSource windAudioSource;
    //Current Gun
    [SerializeField] private AudioSource currentGunAudioSource;
    [SerializeField] private AudioClip currentGunAudioClip;
    //M4A1
    [SerializeField] private AudioSource m4a1AudioSource;
    [SerializeField] private AudioClip m4a1AudioClip;
    //M16
    [SerializeField] private AudioSource m16AudioSource;
    [SerializeField] private AudioClip m16AudioClip;
    //GLOCK
    [SerializeField] private AudioSource glockAudioSource;
    [SerializeField] private AudioClip glockAudioClip;
    //Target Hit
    [SerializeField] private AudioSource targetHitAudioSource;
    [SerializeField] private AudioClip targetHitAudioClip;

    [Header("User Interface")]
    [SerializeField] private GameObject crosshair;
    //Score UI
    [SerializeField] private GameObject roundResultsParent;
    [SerializeField] private Button exitRoundResultsButton;


    [SerializeField] private GameObject scoreUI;

    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private TextMeshProUGUI finalAccuracy;
    [SerializeField] private TextMeshProUGUI finalScore;


    [Header("VFX")]
    [SerializeField] private VisualEffect[] muzzleFlashes = new VisualEffect[2]; //0 = M4A1; 1 = M16; 2 = Glock;
    [SerializeField] private VisualEffect currentMuzzleFlash;
    [SerializeField] private VisualEffect impactParticle;

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
        int rand = Random.Range(0, 2);
        LoadCharacter(rand);
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
        ScoreboardText(timer, accuracy, currentScore);      //Scorboard text
        ShootInput();                                       //Shoot
        EventListeners();
        //Update countdown text
        countdownText.text = countdownStartInitiated ? countdownText.text = countdown.ToString() : countdownText.text = "5";

        if (IsPlaying())
        {

            shootToStartText.gameObject.SetActive(false);   //Enable shoot to start text
            scoreUI.SetActive(true);                        //Enable score UI
            windAudioSource.UnPause();                      //Play wind audio
            crosshair.SetActive(true);                      //Enable crosshair
            countdownStartInitiated = false;                //Determins if countdown start
            if (!decrementing)
            {
                StartCoroutine(GameTimer(1));//Game timer
            }

            if (targetCount < 1)
            {
                HeadshotModePoolManager.instance.GetTarget();    //Get target
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
        finalAccuracy.text = "Final Accuracy: " + accuracy.ToString() + "%";
        roundEnd = false;
        onRoundEnd -= RoundEnd;
        roundResultsParent.SetActive(false);
        currentScore = 0;
        accuracy = 0;
        shootToStartText.gameObject.SetActive(true);    //Enable text


    }

    private void ShootInput()
    {
        //Input
        bool tapInput = Input.GetKeyDown(KeyCode.Mouse0);
        if (tapInput && IsPlaying())
        {
            Shoot();
        }
    }

    private void EnableCountdown()
    {
        //Input
        bool enableGameInput = Input.GetKeyDown(KeyCode.Mouse0);
        if (enableGameInput && !countdownStartInitiated && !roundResultsParent.activeSelf)
        {
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
        timer = 60;                                     //Start game
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
        if (currentMuzzleFlash != null)
        {
            currentMuzzleFlash.Play();
        }

        RaycastHit hit;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward), out hit, 100f))
        {
            totalShotsFired++;
            //Gun Audio Source
            if (currentGunAudioSource != null && currentGunAudioClip != null)
            {
                currentGunAudioSource.PlayOneShot(currentGunAudioClip);
            }
            //Impact Particle
            if (hit.collider != null)
            {
                impactParticle.transform.position = hit.point;
                impactParticle.transform.rotation = Quaternion.LookRotation(hit.normal);
                impactParticle.Play();
            }

            if (hit.collider.CompareTag("Target"))
            {
                AddScore(hit);
            }
            else
            {
                //Reset multiplier
                scoreBonus = 0;
            }
        }
    }

    public void RoundEnd()
    {
        roundResultsParent.SetActive(true);
        finalAccuracy.text = "Final Accuracy: " + currentAccuracy.ToString() + "%";
        finalScore.text = "Final Score: " + currentScore.ToString();

        GameObject[] activeTargets;
        activeTargets = GameObject.FindGameObjectsWithTag("Target");

        for (int i = 0; i < activeTargets.Length; i++)
        {
            activeTargets[i].SetActive(false);
            HeadshotModePoolManager.instance.ReturnTarget(activeTargets[i]);
            targetCount--;
        }
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void AddScore(RaycastHit _hit)
    {
        //Score tracker
        currentScore += targetScoreValue + scoreBonus;
        //Increase multiplier
        if (scoreBonus < 250)
        {
            scoreBonus += 50;
        }
        //Return target
        HeadshotModePoolManager.instance.ReturnTarget(_hit.collider.gameObject);
        //Reduce target count
        targetCount--;
        //Audio 
        targetHitAudioSource.PlayOneShot(targetHitAudioClip);
        //Shots Hit
        totalTargetsHit++;
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

    private float StartGame()
    {
        return timer = 60;
    }

    private void LoadCharacter(int _data)
    {
        if (_data == 0)
        {
            //Load M4A1 Data
            playerCharacters[_data].SetActive(true);
            mainCamera = cams[_data];
            currentGunAudioSource = m4a1AudioSource;
            currentGunAudioClip = m4a1AudioClip;
            currentMuzzleFlash = muzzleFlashes[_data];

        }
        else if (_data == 1)
        {
            //Load M16 Data
            playerCharacters[_data].SetActive(true);
            mainCamera = cams[_data];
            currentGunAudioSource = m16AudioSource;
            currentGunAudioClip = m16AudioClip;
            currentMuzzleFlash = muzzleFlashes[_data];
        }
        else
        {
            //Load Glock Data
            playerCharacters[_data].SetActive(true);
            mainCamera = cams[_data];
            currentGunAudioSource = glockAudioSource;
            currentGunAudioClip = glockAudioClip;
        }
    }

    private void ScoreboardText(int _timer, float _accuracy, int _score)
    {
        //Accuracy calculation + round
        _accuracy = (float)totalTargetsHit * 100 / (float)totalShotsFired;
        float roundAccuracy = Mathf.Round(_accuracy);
        currentAccuracy = roundAccuracy;
        //Set accuracy text
        accuracyText.text = totalTargetsHit > 0 && totalShotsFired > 0 ? accuracyText.text = roundAccuracy.ToString() + "%" : accuracyText.text = "0%";
        //Set timer text
        timerText.text = _timer > 0 ? timerText.text = _timer.ToString() : timerText.text = "0";
        //Set Score Text
        scoreText.text = _score > 0 ? scoreText.text = _score.ToString() : scoreText.text = "0";
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
