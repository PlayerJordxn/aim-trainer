using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;
using UnityEngine.Events;
using UnityEditor;




public class GridshotManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera[] cams;//0 = M4A1; 1 = M16; 2 = Glock;
    [SerializeField] private Camera mainCamera;

    [Header("Characters")]
    [SerializeField] private GameObject[] playerCharacters = new GameObject[3]; //0 = M4A1; 1 = M16; 2 = Glock;
    public PlayerController playerController;

    private GameObject activeArmature;

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
        int rand = UnityEngine.Random.Range(0,2);
        //LoadCharacter(rand);
        // Player controller has to be moved into awake in order for loadCharacterV2 to work
        playerController = FindObjectOfType<PlayerController>();
        LoadCharacterV2();
    }

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //playerController = FindObjectOfType<PlayerController>();
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
           
            if (targetCount < 5)
            {
                ObjectPool.instance.GetTarget();    //Get target
                targetCount++;                      //Increment target count
            }
        }
        else
        {
            if(roundEnd)
            {
                onRoundEnd += RoundEnd;
                roundEnd = false;
            }
            if (!countdownStartInitiated)
            {
                countdown = 5;                              //Reset countdown
            }
            countdownText.text = countdown.ToString();      //Update countdown text
            shootToStartText.gameObject.SetActive(true);    //Enable text
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
        if(currentMuzzleFlash != null)
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
            if(hit.collider != null)
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

        for(int i = 0; i < activeTargets.Length; i++)
        {
            activeTargets[i].SetActive(false);
            ObjectPool.instance.ReturnTarget(activeTargets[i]);
            targetCount--;
        }
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void AddScore(RaycastHit _hit)
    {
        //Score tracker
        currentScore += targetScoreValue + scoreBonus;
        //Increase multiplier
        if(scoreBonus < 250)
        {
            scoreBonus += 50;
        }
        //Return target
        ObjectPool.instance.ReturnTarget(_hit.collider.gameObject);
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

    private void LoadCharacterV2()
    {
        // Edgecase where gun doesn't exist?
        /*var currentGun = SaveSystem.LoadGunData();
        if (currentGun == null)
        {
            return; //default to a base gun and values
        }*/

        /*
         * *NOTE: For this system to work, it needs the armatures to be placed in a resource folder.
         * Typically the folder structure would be Assets -> Resources -> ...
         * For now, the weapons are found in the Test_armatures folder in Resources
         * 
         * For production, remove demo code below and load the specific armature found in curweapon.bin
         * 
         * All armatures that are found in Test_Armatures have to have the weapon tagged as "Weapon",
         * and the root of the armature needs to have an empty gameobject containing the audio source
         * 
         * If a weapon has a muzzle flash, the muzzle flash has to be titled "Muzzle Flash" for this code
         * to find it correctly.
         */

        // Demos represent how weapons will be loaded.
        // DEMO - display random weapon
        // Load available armatures from the "Test_Armatures" folder through a resource load
        var selections = Resources.LoadAll("Test_Armatures", typeof(GameObject));
        var prefab = selections[UnityEngine.Random.Range(0, selections.Length)] as GameObject;
        
        // DEMO 2 - display specific weapon
        //var armatureName = "GLOCK_Armature";
        //var prefab = Resources.Load("Test_Armatures/" + armatureName) as GameObject;
        var armatureObject = GameObject.Find("Player/Armature").transform;
        
        if (prefab != null)
        {
            activeArmature = Instantiate(prefab, armatureObject);
            // Change the name of the loaded armature to be MUCH easier to target/deal with
            activeArmature.name = "Loaded_Armature";
        }
        
        // Find ground, spine, camera, audio source and weapon in loaded armature
        var groundCheck = armatureObject.Find("GroundCheck");
        var armatureSpine = activeArmature.transform.Find("metarig/spine/spine.001");
        var armatureCam = armatureSpine.Find("Camera").gameObject;
        var prefabAudioSource = activeArmature.transform.Find("Audio").GetComponent<AudioSource>();

        // Find the weapon in the armature so we can figure out if a muzzle flash exists
        var activeWeapon = FindDeepChildByTag(activeArmature.transform, "Weapon", true);
        var muzzleFlash = activeWeapon.Find("MuzzleFlashTransform/Muzzle Flash");

        // Access PlayerController and set spine and groundCheck
        playerController.spine001 = armatureSpine;
        playerController.groundCheck = groundCheck;

        mainCamera = armatureCam.GetComponent("Camera") as Camera;

        // Set audio source and clip using the same Audio gameobject in the armature
        currentGunAudioSource = prefabAudioSource;
        currentGunAudioClip = prefabAudioSource.clip;
        // If we have a muzzle flash, set it, else ignore
        if (muzzleFlash)
        {
            currentMuzzleFlash = muzzleFlash.GetComponent<VisualEffect>();
        }

    }

    // This is a generic search algorithm to find a single child within a nested tree
    // To summize, it checks all child objects, then checks their children and so on until theres none
    // left or the tag is found.

    // There are 2 modes, breathfirst vs depthfirst (represented by the breadthFirst bool)
    // If breadthfirst, the algorithm will check each LAYER first before going the the next layer of children
    // If depthfirst, the alogirthm will check each CHILD of an object before moving on.

    // Generally speaking, imagine breadthfirst as a bush, and a depthfirst as a vine.
    public Transform FindDeepChildByTag(Transform parent, string tag, bool mustBeActive, bool breadthFirst = false)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if ((parent.GetChild(i).CompareTag(tag)) && (!mustBeActive || (parent.GetChild(i).gameObject.activeSelf)))
            {
                return parent.GetChild(i);
            }
            if (!breadthFirst)
            {
                Transform grandchild = FindDeepChildByTag(parent.GetChild(i), tag, breadthFirst);
                if ((grandchild != null) && (!mustBeActive || (grandchild.gameObject.activeSelf)))
                    return grandchild;
            }
        }
        if (breadthFirst)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform grandchild = FindDeepChildByTag(parent.GetChild(i), tag, breadthFirst);
                if ((grandchild != null) && (!mustBeActive || (grandchild.gameObject.activeSelf)))
                    return grandchild;
            }
        }
        return null;
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
        if(timer == 1)
        {
            roundEnd = true;
        }
        decrementing = false;
    }

    private void SaveRoundReslut()
    {

    }
}
   

