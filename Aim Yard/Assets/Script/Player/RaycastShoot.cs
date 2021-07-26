using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RaycastShoot : MonoBehaviour
{
    //Script Accsess
    Gridshot gridshot;
    GridshotSpawner gridshotSpawner;

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

    //UI
    [SerializeField] Image hitMarker;
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
        gridshotSpawner = FindObjectOfType<GridshotSpawner>();
        gridshot = FindObjectOfType<Gridshot>();

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
        
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            glockSFX.PlayOneShot(glockClipSFX);
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
                    gridshot.ReturnTarget(hit.collider.gameObject);
                    gridshotSpawner.targetsInScene--;
                }

                if (colorCordinationIsPlaying)
                {
                    //COLOR CORDINATION
                    colorCordination.ReturnTarget(hit.collider.gameObject);
                    colorCordinationSpawner.targetsInScene--;
                }

                if(headshotModeIsPlaying)
                {
                    headshotMode.ReturnTarget(hit.collider.gameObject);
                    headshotModeSpawner.targetsInScene--;
                }

                if(flickshotModeIsPlaying)
                {
                    flickshotMode.ReturnTarget(hit.collider.gameObject);
                    flickshotSpawner.targetsInScene--;
                }

                if (rangeTrainingIsPlaying)
                {
                    rangeTrainingManager.ReturnTarget(hit.collider.gameObject);
                    rangeTrainingSpawner.targetsInScene--;
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
