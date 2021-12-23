using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;

public class RaycastShoot : MonoBehaviour
{
    [SerializeField] private VisualEffect m4MuzzleFlash;
    [SerializeField] private VisualEffect m16MuzzleFlash;

    [SerializeField] private VisualEffect impactParticle;

    public static RaycastShoot instance;

    //Audio Source
    [SerializeField] private AudioSource glockSFX;
    [SerializeField] private AudioClip glockClipSFX;

    [SerializeField] private AudioSource m16SFX;
    [SerializeField] private AudioClip m16ClipSFX;

    [SerializeField] private AudioSource m4a1SFX;
    [SerializeField] private AudioClip m4a1ClipSFX;

    [SerializeField] private AudioSource targetHitSFX;
    [SerializeField] private AudioClip targetHitClipSFX;

    //UI
    private float time;
    private float duration;

    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Text sensitivityText;


    //Raycast Variables
    [SerializeField] Camera lookFrom;
    private float shootDistance;

    //Gamemodes
    public bool gridshotIsPlaying;
    public bool colorCordinationIsPlaying;
    public bool headshotModeIsPlaying;
    public bool flickshotModeIsPlaying;
    public bool singleTargetTrackingIsPlaying;
    public bool rangeTrainingIsPlaying;
    public bool colorCordinationTrackingIsPlaying;

    public bool gameStarted = false;
    public bool paused = false;
    

    //Weapons - Determines what audio should be played
    public bool playM4Audio = false;
    public bool playGlockAudio = false;
    public bool playM16Audio = false;

    //Score
    public float missed = 0;
    public float shotsHit = 0;
    public float accuracy = 0;

    //Lifetime Statistics
    [SerializeField] private Text StatsMissedText;
    [SerializeField] private Text StatsHitText;
    [SerializeField] private Text StatsAccuracyText;
    private float lifetimeMissedShots = 0;
    private float lifetimeHitShots = 0;
    private float lifetimeAccuracy = 0;



    [SerializeField] private  Text shotsFiredText;
    [SerializeField] private Text shotsHitText;
    [SerializeField] private Text accuracyText;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }

        gridshotIsPlaying = false;
        colorCordinationIsPlaying = false;
        headshotModeIsPlaying = false;
        flickshotModeIsPlaying = false;
        singleTargetTrackingIsPlaying = false;
        colorCordinationTrackingIsPlaying = false;

       
    }
    // Start is called before the first frame update
    void Start()
    {
        /*
        if (PlayerPrefs.GetFloat("Sensitivity") == 0)
        {
            sensitivitySlider.value = 5;
            PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
        }
        else
        {
            CharcterCamera.instance.mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity");
        }
        */
        sensitivitySlider.value = 5;

        if (accuracy <= 0)
            accuracy = 100f;

        if (shootDistance <= 0)
            shootDistance = 500f;

        if (duration <= 0)
            duration = 2f;
    }

    // Update is called once per frame
    void Update()
    {

        CharcterCamera.instance.mouseSensitivity = sensitivitySlider.value;
        sensitivityText.text = sensitivitySlider.value.ToString();
        //Sensitivity
        //PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
        //CharcterCamera.instance.mouseSensitivity = PlayerPrefs.GetFloat("Sensititvty");


        if (shotsHit > 0 && missed > 0)
        {
            ScoreUI(shotsFiredText, shotsHitText, accuracyText, shotsHit, missed, accuracy);
            PlayerStatistics(StatsMissedText, StatsHitText, StatsAccuracyText, shotsHit, missed, accuracy);
        } 
        else
        {
            //Text
            shotsFiredText.text = "Missed: 0";
            shotsHitText.text = "Hit: 0";
            accuracyText.text = "Accuaracy: 0%";

            StatsMissedText.text = "Missed: 0";
            StatsHitText.text = "Hit: 0";
            StatsAccuracyText.text = "Accuaracy: 0%";
        }


            

       

        if (Input.GetButtonDown("Fire1") && !paused && gameStarted)
        {
            
            //SFX
            if (playGlockAudio)
                glockSFX.PlayOneShot(glockClipSFX);
                
            if (playM16Audio)
            {
                m16SFX.PlayOneShot(m16ClipSFX);
                m16MuzzleFlash.Play();
            }

            if (playM4Audio)
            {
                m4MuzzleFlash.Play();
                m4a1SFX.PlayOneShot(m4a1ClipSFX);
            }
        
            Shoot();
            
        }
       
    }

    public void Shoot()
    {
        RaycastHit hit;

        if(Physics.Raycast(lookFrom.transform.position, lookFrom.transform.forward, out hit, shootDistance))
        {
            if(hit.collider != null)
            {
                impactParticle.transform.position = hit.point;
                impactParticle.transform.rotation = Quaternion.LookRotation(hit.normal);
                impactParticle.Play();
            }

            if (hit.collider.tag != "Target")
            {
                missed++;
            }

            if (hit.collider.tag == "Target")
            {
                if (gridshotIsPlaying)
                {   
                    //GRIDSHOT
                    Gridshot.instance.ReturnTarget(hit.collider.gameObject);
                    GridshotSpawner.instance.targetsInScene--;
                    

                    //Audio
                    targetHitSFX.PlayOneShot(targetHitClipSFX);

                    //Score
                    shotsHit++;
                    //lifetimeHitShots++;

                }

                if (colorCordinationIsPlaying)
                {
                    //COLOR CORDINATION
                    ColorCordination.instance.ReturnTarget(hit.collider.gameObject);
                    ColorCordinationSpawner.instance.targetsInScene--;

                    //Audio
                    targetHitSFX.PlayOneShot(targetHitClipSFX);

                    //Score
                    shotsHit++;
                    //lifetimeHitShots++;

                }

                if (headshotModeIsPlaying)
                {
                    //HEADSHOT MODE
                    HeadshotMode.instance.ReturnTarget(hit.collider.gameObject);
                    HeadshotModeSpawner.instance.targetsInScene--;

                    //Audio
                    targetHitSFX.PlayOneShot(targetHitClipSFX);

                    //Score
                    shotsHit++;
                    //lifetimeHitShots++;

                }

                if (flickshotModeIsPlaying)
                {
                    //FLICKSHOT MODE
                    Flickshot.instance.ReturnTarget(hit.collider.gameObject);
                    FlickshotSpawner.instance.targetsInScene--;

                    //Audio
                    targetHitSFX.PlayOneShot(targetHitClipSFX);

                    //Score
                    shotsHit++;
                    //lifetimeHitShots++;

                }

                if (rangeTrainingIsPlaying)
                {
                    //RANGE TRAINING
                    RangeTrainingMode.instance.ReturnTarget(hit.collider.gameObject);
                    RangeTrainingSpawner.instance.targetsInScene--;

                    //Audio
                    targetHitSFX.PlayOneShot(targetHitClipSFX);

                    //Score
                    shotsHit++;
                    //lifetimeHitShots++;
                }
                missed++;
            }
        }
    }

    public void Tracking()
    {
        RaycastHit hit;

        if (Physics.Raycast(lookFrom.transform.position, lookFrom.transform.forward, out hit, shootDistance))
        {
            if (hit.collider.tag == "TrackingTarget")
            {
                if(singleTargetTrackingIsPlaying)
                {
                    hit.collider.gameObject.GetComponent<TargetBehavior>().RemoveHealthSingleTarget(hit.collider.gameObject);
                }

                if (colorCordinationTrackingIsPlaying)
                {
                    hit.collider.gameObject.GetComponent<TargetBehavior>().RemoveHealthColorCordinationTracking(hit.collider.gameObject);
                }
            }
        }
    }

    private void ScoreUI(Text _shotsFiredText, Text _shotsHitText, Text _accuracyText, float _hit, float _missed, float _accuracy)
    {
        //Text
        _shotsFiredText.text = "Missed: " + missed.ToString();
        _shotsHitText.text = "Hit: " +_hit.ToString();

        //Accuracy
        //Calcuate Percent
        float percent = (shotsHit / missed) * 100.0f;
        float round = Mathf.Round(percent);
            
        _accuracyText.text = "Accuaracy: " + round.ToString() + "%";
        
    }

    private void PlayerStatistics(Text _shotsFiredText, Text _shotsHitText, Text _accuracyText, float _hit, float _missed, float _accuracy)
    {
        //Text
        _shotsFiredText.text = missed.ToString();
        _shotsHitText.text = _hit.ToString();

        //Accuracy
        //Calcuate Percent
        float percent = (shotsHit / missed) * 100.0f;
        float round = Mathf.Round(percent);

        _accuracyText.text = round.ToString() + "%";

    }
}
