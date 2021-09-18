using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RaycastShoot : MonoBehaviour
{
    //Script Accsess
    ColorCordination colorCordination;
    ColorCordinationSpawner colorCordinationSpawner;

    HeadshotMode headshotMode;
    HeadshotModeSpawner headshotModeSpawner;

    Flickshot flickshotMode;
    FlickshotSpawner flickshotSpawner;

    SingleTargetTracking singleTargetTracking;
    SingleTrackingTargetSpawner singleTargetSpawner;
    TargetBehavior targetBehaviourScript;

    RangeTrainingMode rangeTrainingManager;
    RangeTrainingSpawner rangeTrainingSpawner;

    ColorCordinationTracking colorCordinationTracking;
    ColorCordinationTrackingSpawner colorCordinationTrackingSpawner;
   

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
    public bool m4a1SfxBool = false;
    public bool glockSfxBool = false;
    public bool m16SfxBool = false;

    //Score
    public float shotsFired = 0;
    public float shotsHit = 0;
    public float accuracy = 0;

    [SerializeField] TextMeshProUGUI shotsFiredText;
    [SerializeField] TextMeshProUGUI shotsHitText;
    [SerializeField] TextMeshProUGUI accuracyText;

    private void Awake()
    {
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
        colorCordination = FindObjectOfType<ColorCordination>();
        colorCordinationSpawner = FindObjectOfType<ColorCordinationSpawner>();

        headshotMode = FindObjectOfType<HeadshotMode>();
        headshotModeSpawner = FindObjectOfType<HeadshotModeSpawner>();

        flickshotMode = FindObjectOfType<Flickshot>();
        flickshotSpawner = FindObjectOfType<FlickshotSpawner>();

        singleTargetTracking = FindObjectOfType<SingleTargetTracking>();
        targetBehaviourScript = FindObjectOfType<TargetBehavior>();

        rangeTrainingManager = FindObjectOfType<RangeTrainingMode>();
        rangeTrainingSpawner = FindObjectOfType<RangeTrainingSpawner>();

        colorCordinationTracking = FindObjectOfType<ColorCordinationTracking>();
        colorCordinationTrackingSpawner = FindObjectOfType<ColorCordinationTrackingSpawner>();

        

        if (shootDistance <= 0)
            shootDistance = 150f;

        if (duration <= 0)
            duration = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(shotsFired > 0 && shotsHit > 0)
        {
            //Calcuate Percent
            float percent = (shotsHit / shotsFired) * 100.0f;
            float round = Mathf.Round(percent);
            accuracyText.text = "Accuaracy: " + round.ToString() + "%";
        }
        
        shotsFiredText.text = shotsFired.ToString();
        shotsHitText.text = shotsHit.ToString();

        if (Input.GetButtonDown("Fire1"))
        {
            //SFX
            if (glockSfxBool)
                glockSFX.PlayOneShot(glockClipSFX);

            if (m16SfxBool)
                m16SFX.PlayOneShot(m16ClipSFX);

            if (m4a1SfxBool)
                m4a1SFX.PlayOneShot(m4a1ClipSFX);

            Shoot();

            if (GridshotSpawner.instance.isPlaying || colorCordinationIsPlaying || flickshotModeIsPlaying || headshotModeIsPlaying || rangeTrainingIsPlaying)
                shotsFired++;

        }
        
        //Tracking();
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
                    colorCordination.ReturnTarget(hit.collider.gameObject);
                    colorCordinationSpawner.targetsInScene--;

                    //Audio
                    targetHitSFX.PlayOneShot(targetHitClipSFX);

                    //Score
                    shotsHit++;
                }

                if(headshotModeIsPlaying)
                {
                    headshotMode.ReturnTarget(hit.collider.gameObject);
                    headshotModeSpawner.targetsInScene--;

                    //Audio
                    targetHitSFX.PlayOneShot(targetHitClipSFX);

                    //Score
                    shotsHit++;
                }

                if(flickshotModeIsPlaying)
                {
                    flickshotMode.ReturnTarget(hit.collider.gameObject);
                    flickshotSpawner.targetsInScene--;

                    //Audio
                    targetHitSFX.PlayOneShot(targetHitClipSFX);

                    //Score
                    shotsHit++;
                }

                if (rangeTrainingIsPlaying)
                {
                    rangeTrainingManager.ReturnTarget(hit.collider.gameObject);
                    rangeTrainingSpawner.targetsInScene--;

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
}
