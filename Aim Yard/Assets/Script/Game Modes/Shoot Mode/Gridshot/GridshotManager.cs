using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class GridshotManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera[] cams;//0 = M4A1; 1 = M16; 2 = Glock;
    [SerializeField] private Camera mainCamera;

    [Header("Characters")]
    [SerializeField] private GameObject[] playerCharacters = new GameObject[3]; //0 = M4A1; 1 = M16; 2 = Glock;

    [Header("Game Settings")]
    private int targetCount;
    private float accuracy = 0f;
    private int currentScore = 0;
    private int timer = 0;
    private bool isPlaying = false;

    [Header("Score")]
    private float previousScore = 0f;
    private int scoreBonus = 0;
    private int targetScoreValue = 100;
    private bool decrementing = false;
    private int totalTargetsHit = 0;
    private int totalShotsFired = 0;

   
   
    [Header("Audio")]
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
    [SerializeField] private GameObject scoreUI;

    [SerializeField] private Button playButton;
    [SerializeField] private GameObject canvas;

    [SerializeField] private TextMeshProUGUI primaryWeaponLoadoutText;
    [SerializeField] private TextMeshProUGUI secondaryWeaponLoadoutText;

    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;



    void Awake()
    {
        //PlayerPrefs.GetInt("Character")
        LoadCharacter(1);
    }
    // Start is called before the first frame update
    void Start()
    {
        //User Interface 
        if(playButton)
        {
            playButton.onClick.AddListener(delegate { StartGame(); });
        }
    }

    // Update is called once per frame
    void Update()
    {
        Scoreboard(timer, accuracy, currentScore);
        ShootInput();

        if (IsPlaying())
        {
            //Enable in game score UI
            scoreUI.SetActive(true);
            //UnPause wind audio
            windAudioSource.UnPause();
            //Disable start screen canvas
            canvas.SetActive(false);
            //Enable crosshair
            crosshair.SetActive(true);

            //Timer
            if(!decrementing)
            {
                StartCoroutine(GameTimer(1));
            }
           
            if (targetCount < 5)
            {
                //Spawn target
                ObjectPool.instance.GetTarget();

                //Add to count
                targetCount++;
            }
        }
        else
        { 
            //Disable in game score UI
            scoreUI.SetActive(false);
            //Pause wind audio
            windAudioSource.Pause();
            //Enable start screen canvas
            canvas.SetActive(true);
            //Disable crosshair
            crosshair.SetActive(true);
        }

        
    }

    private void ShootInput()
    {
        //Input
        var tapInput = Input.GetKeyDown(KeyCode.Mouse0);
        
        if (tapInput && IsPlaying())
        {
            Shoot();
        }
    }   
    private void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward), out hit, 100f))
        {
            totalShotsFired++;
            //Gun Audio Source
            if (currentGunAudioSource != null && currentGunAudioClip != null)
            {
                currentGunAudioSource.PlayOneShot(currentGunAudioClip);               
            }

            //Target Hit
            if (hit.collider.CompareTag("Target"))
            {
                //Score tracker
                currentScore += targetScoreValue + scoreBonus;
                //Increase multiplier
                if(scoreBonus < 250)
                {
                    scoreBonus += 50;
                }
                
                //Return target
                ObjectPool.instance.ReturnTarget(hit.collider.gameObject);
                //Reduce target count
                targetCount--;
                //Audio 
                targetHitAudioSource.PlayOneShot(targetHitAudioClip);
                //Shots Hit
                totalTargetsHit++;
            }

            if (!hit.collider.CompareTag("Target"))
            {
                //Reset multiplier
                scoreBonus = 0;
            }
        }
    }

    public bool IsPlaying()
    {
        if (timer > 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            return isPlaying = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
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
            primaryWeaponLoadoutText.text = "M4A1";
            secondaryWeaponLoadoutText.text = "EMPTY";
        }
        else if (_data == 1)
        {
            //Load M16 Data
            playerCharacters[_data].SetActive(true);
            mainCamera = cams[_data];
            currentGunAudioSource = m16AudioSource;
            currentGunAudioClip = m16AudioClip;
            primaryWeaponLoadoutText.text = "M16";
            secondaryWeaponLoadoutText.text = "EMPTY";
        }
        else
        {
            //Load Glock Data
            playerCharacters[_data].SetActive(true);
            mainCamera = cams[_data];
            currentGunAudioSource = glockAudioSource;
            currentGunAudioClip = glockAudioClip;
            primaryWeaponLoadoutText.text = "EMPTY";
            secondaryWeaponLoadoutText.text = "GLOCK";
        }
    }

    private void Scoreboard(int _timer, float _accuracy, int _score)
    {
        if(timer > 0)
        {
            timerText.text = _timer.ToString();
        }
        else
        {
            timerText.text = "0";
        }

        if(totalTargetsHit > 0 && totalShotsFired > 0)
        {
            _accuracy = (float)totalTargetsHit * 100 / (float)totalShotsFired;
            var roundAccuracy = Mathf.Round(_accuracy);
            accuracyText.text = roundAccuracy.ToString();

        }
        else
        {
            accuracyText.text = "0";
        }

        if(currentScore > 0)
        {
            scoreText.text = _score.ToString();
        }
        else
        {
            scoreText.text = "0";

        }

    }

    private IEnumerator GameTimer(int _wait)
    {
        print("CALLED");
        decrementing = true;
        yield return new WaitForSecondsRealtime(_wait);
        timer--;
        decrementing = false;
    }

    private void RoundResult()
    {

    }

    private void PreviousRoundResult()
    {

    }

    private void SaveRoundReslut()
    {

    }






}
   

