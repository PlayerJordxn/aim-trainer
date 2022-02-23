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
    private bool isShooting = false;
    private bool isPlaying = false;
    private bool glockActive, m16Active, m4a1Active;

    [Header("Score")]
    private float currentScore = 0f;
    private float previousScore = 0f;
    private float scoreMultiplier = 0;
    private float ScoreMultiplier
    {
        get { return scoreMultiplier; }
        set { scoreMultiplier = Mathf.Clamp(value, 0, 500); }
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
    //Target Hit
    [SerializeField] private AudioSource targetHitAudioSource;
    [SerializeField] private AudioClip targetHitAudioClip;

    [Header("User Interface")]
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject canvas;



    

    void Awake()
    {
        //PlayerPrefs.GetInt("Character")
        LoadCharacter(1);
    }
    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;
        
        for(int i = 0; i < cams.Length; i++)
        {
            
            GameObject.FindGameObjectWithTag("Cam" + i.ToString());
        }
       
        if(playButton)
        {
            playButton.onClick.AddListener(delegate { StartGame(); });
        }
    }

    // Update is called once per frame
    void Update()
    {

        print("Playing: " + timer);
        ShootInput();

        if (IsPlaying())
        {
            //Disable Canvas
            canvas.SetActive(false);

            //Timer
            timer -= Time.deltaTime;

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
            canvas.SetActive(true);
        }

        if (timer <= 0)
            timer = 0f;
        else if (timer > 60)
            timer = 60f;
    }

    private void ShootInput()
    {
        //Input
        var tapInput = Input.GetKeyDown(KeyCode.Mouse0);
        var sprayInput = Input.GetButton("Fire1");
        
        

        if (tapInput && IsPlaying())
        {
            if(glockActive)
            {
                Shoot();
            }
        }

        if (sprayInput)
        {
            isShooting = true;
            if (IsPlaying())
            {
                if (m4a1Active || m16Active)
                {
                    Shoot();
                }
            }
        }
        else
        {
            isShooting = false;
        }
    }   
    private void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward), out hit, 100f))
        {
            //Gun Audio Source
            if (currentGunAudioSource != null && currentGunAudioClip != null)
            {
                if(glockActive)
                    currentGunAudioSource.PlayOneShot(currentGunAudioClip);

                if (m16Active)
                    currentGunAudioSource.Play();

                if (m4a1Active)
                    currentGunAudioSource.Play();
            }

            //Target Hit
            if (hit.collider.CompareTag("Target"))
            {
                //Score tracker
                currentScore = targetScoreValue + scoreMultiplier;
                scoreMultiplier += 50f;
                //Return target
                ObjectPool.instance.ReturnTarget(hit.collider.gameObject);
                //Reduce target tount
                targetCount--;
                //Increase multiplier
                scoreMultiplier++;
                //Audio 
                targetHitAudioSource.PlayOneShot(targetHitAudioClip);
            }

            if (!hit.collider.CompareTag("Target"))
            {
                //Reset multiplier
                scoreMultiplier = 1;
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
        return timer = 60f;
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
            m4a1Active = true;
        }
        else if (_data == 1)
        {
            //Load M16 Data
            playerCharacters[_data].SetActive(true);
            mainCamera = cams[_data];
            currentGunAudioSource = m16AudioSource;
            currentGunAudioClip = m16AudioClip;
            m16Active = true;
        }
        else
        {

            //Load Glock Data
            playerCharacters[_data].SetActive(true);
            mainCamera = cams[_data];
            currentGunAudioSource = glockAudioSource;
            currentGunAudioClip = glockAudioClip;
            glockActive = true;
        }
    }

   


}
   

