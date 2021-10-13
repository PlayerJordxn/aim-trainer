using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RaycastShoot : MonoBehaviour
{
    public static RaycastShoot instance;
    
    //Audio Source
    [SerializeField] AudioSource glockSFX;
    [SerializeField] AudioClip glockClipSFX;

    [SerializeField] AudioSource m16SFX;
    [SerializeField] AudioClip m16ClipSFX;

    [SerializeField] AudioSource m4a1SFX;
    [SerializeField] AudioClip m4a1ClipSFX;

    [SerializeField] AudioSource targetHitSFX;
    [SerializeField] AudioClip targetHitClipSFX;

    //UI
    float time;
    float duration;

    //Raycast Variables
    [SerializeField] Camera lookFrom;
    float shootDistance;

    //Gamemodes
    public bool gridshotIsPlaying;
    public bool colorCordinationIsPlaying;
    public bool headshotModeIsPlaying;
    public bool flickshotModeIsPlaying;
    public bool singleTargetTrackingIsPlaying;
    public bool rangeTrainingIsPlaying;
    public bool colorCordinationTrackingIsPlaying;

    //Weapons - Determines what audio should be played
    public bool playM4Audio = false;
    public bool playGlockAudio = false;
    public bool playM16Audio = false;

    //Score
    public float shotsFired = 0;
    public float shotsHit = 0;
    public float accuracy = 0;

    [SerializeField] Text shotsFiredText;
    [SerializeField] Text shotsHitText;
    [SerializeField] Text accuracyText;

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

        if (shootDistance <= 0)
            shootDistance = 150f;

        if (duration <= 0)
            duration = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        ScoreUI(shotsFiredText, shotsHitText, accuracyText, shotsHit, shotsFired, accuracy);

        if (Input.GetButtonDown("Fire1"))
        {
            //SFX
            if (playGlockAudio)
                glockSFX.PlayOneShot(glockClipSFX);

            if (playM16Audio)
                m16SFX.PlayOneShot(m16ClipSFX);

            if (playM4Audio)
                m4a1SFX.PlayOneShot(m4a1ClipSFX);

            Shoot();

            if (gridshotIsPlaying 
                || colorCordinationTrackingIsPlaying 
                || flickshotModeIsPlaying 
                || headshotModeIsPlaying 
                || rangeTrainingIsPlaying)
                shotsFired++;

        }
       
    }

    public void Shoot()
    {
        RaycastHit hit;

        if(Physics.Raycast(lookFrom.transform.position, lookFrom.transform.forward, out hit, shootDistance))
        {
            if(hit.collider.tag == "Target")
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
                }

                if(headshotModeIsPlaying)
                {
                    //HEADSHOT MODE
                    HeadshotMode.instance.ReturnTarget(hit.collider.gameObject);
                    HeadshotModeSpawner.instance.targetsInScene--;

                    //Audio
                    targetHitSFX.PlayOneShot(targetHitClipSFX);

                    //Score
                    shotsHit++;
                }

                if(flickshotModeIsPlaying)
                {
                    //FLICKSHOT MODE
                    Flickshot.instance.ReturnTarget(hit.collider.gameObject);
                    FlickshotSpawner.instance.targetsInScene--;

                    //Audio
                    targetHitSFX.PlayOneShot(targetHitClipSFX);

                    //Score
                    shotsHit++;
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
                }

                
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

    public void ScoreUI(Text _shotsFiredText, Text _shotsHitText, Text _accuracyText, float _hit, float _shotsFired, float _accuracy)
    {
        //Text
        _shotsFiredText.text = _shotsFired.ToString();
        _shotsHitText.text = _hit.ToString();

        //Accuracy
        if (shotsFired > 0 && shotsHit > 0)
        {
            //Calcuate Percent
            float percent = (shotsHit / shotsFired) * 100.0f;
            float round = Mathf.Round(percent);
            
            _accuracyText.text = "Accuaracy: " +
                "" + round.ToString() + "%";
        }
    }
}
