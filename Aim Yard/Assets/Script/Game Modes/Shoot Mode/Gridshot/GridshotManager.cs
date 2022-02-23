using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GridshotManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera[] cams;//0 = M4A1; 1 = M16; 2 = Glock;
    [SerializeField] private Camera mainCamera;

    [Header("Characters")]
    [SerializeField] private GameObject[] playerCharacters = new GameObject[3]; //0 = M4A1; 1 = M16; 2 = Glock;

    [Header("Game Settings")]
    [SerializeField] private int targetCount;
    private float timer = 0f;
    private bool isPlaying = false;
    private bool isShooting = false;

    [Header("Score")]
    private float currentScore = 0f;
    private float previousScore = 0f;
    private int scoreMultiplier = 1;
    private int ScoreMultiplier
    {
        get { return scoreMultiplier; }
        set { scoreMultiplier = Mathf.Clamp(value, 1, 5); }
    }
    private float targetScoreValue = 250; 
   
    [Header("Audio")]
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


    // Start is called before the first frame update
    void Start()
    {
        //LoadCharacter(PlayerPrefs.GetInt("Character"));
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && isPlaying && !isShooting)
        {
            isShooting = true;
            Shoot();
        }

        //Determimes if the game is playing
        if(timer > 0)
        {
            isPlaying = true;
        }
        else
        {
            isPlaying = false;
        }

        if(isPlaying)
        {
            //Timer
            timer -= Time.time;

            if(targetCount < 5)
            {
                //Spawn target
                ObjectPool.instance.GetTarget();

                //Add to count
                targetCount++;
            }
        }
        else
        {
            //Reset Timer
            timer = 60f;
        }
    }

    public void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward), out hit, 100f))
        {
            //Gun Audio Source
            if(currentGunAudioSource != null && currentGunAudioClip != null)
            {
                currentGunAudioSource.PlayOneShot(currentGunAudioClip);
            }

            //Target Hit
            if(hit.collider.CompareTag("Target"))
            {
                //Score tracker
                currentScore += targetScoreValue * scoreMultiplier;
                //Return target
                ObjectPool.instance.ReturnTarget(hit.collider.gameObject);
                //Reduce target tount
                targetCount--;
                //Increase multiplier
                scoreMultiplier++;
            }

            if (!hit.collider.CompareTag("Target"))
            {
                //Reset multiplier
                scoreMultiplier = 1;
            }
        }
        isShooting = false;
    }

    private void LoadCharacter(int _data)
    {
        if(_data == 0)
        {
            //Load M4A1 Data
            playerCharacters[0].SetActive(true);
            mainCamera = cams[0];
            currentGunAudioSource = m4a1AudioSource;
            currentGunAudioClip = m4a1AudioClip;
        }
        else if( _data == 1)
        {
            //Load M16 Data
            playerCharacters[0].SetActive(true);
            mainCamera = cams[1];
            currentGunAudioSource = m16AudioSource;
            currentGunAudioClip = m16AudioClip;
        }
        else
        {
            //Load Glock Data
            playerCharacters[0].SetActive(true);
            mainCamera = cams[2];
            currentGunAudioSource = glockAudioSource;
            currentGunAudioClip = glockAudioClip;
        }
    }

   
}
