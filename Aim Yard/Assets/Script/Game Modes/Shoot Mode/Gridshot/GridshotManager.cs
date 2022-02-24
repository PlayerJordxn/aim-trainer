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
    private float timer = 0f;
    private float Timer
    {
        get { return timer; }
        set { timer = Mathf.Clamp(value, 0f, 60f); }
    }
    private bool isPlaying = false;

    [Header("Score")]
    private float currentScore = 0f;
    private float previousScore = 0f;
    private float scoreBonus = 0;
    private float ScoreBonus
    { 
        get { return scoreBonus; }
        set { scoreBonus = Mathf.Clamp(value, 0, 500); }
    }
    private float targetScoreValue = 250; 
   
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
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject canvas;

    [SerializeField] private TextMeshProUGUI primaryWeaponLoadoutText;
    [SerializeField] private TextMeshProUGUI secondaryWeaponLoadoutText;

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
        ShootInput();

        if (IsPlaying())
        {
            //UnPause wind audio
            windAudioSource.UnPause();
            //Disable start screen canvas
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
            //Pause wind audio
            windAudioSource.Pause();
            //Enable start screen canvas
            canvas.SetActive(true);

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
            //Gun Audio Source
            if (currentGunAudioSource != null && currentGunAudioClip != null)
            {
                currentGunAudioSource.PlayOneShot(currentGunAudioClip);               
            }

            //Target Hit
            if (hit.collider.CompareTag("Target"))
            {
                //Score tracker
                currentScore = targetScoreValue + scoreBonus;
                //Increase multiplier
                scoreBonus += 50f;
                //Return target
                ObjectPool.instance.ReturnTarget(hit.collider.gameObject);
                //Reduce target count
                targetCount--;
                //Audio 
                targetHitAudioSource.PlayOneShot(targetHitAudioClip);
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
   

