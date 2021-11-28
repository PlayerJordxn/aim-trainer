using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GridshotSpawner : MonoBehaviour
{
    public static GridshotSpawner instance;
    [SerializeField] Animator anim;

    public int targetsInScene = 0;
    public bool isPlaying;

    public int timeLeft = 60;
    public bool isDecrementing = false;
    private bool lockCursor = false;
    public int weaponShowcase = 0; //0 = M16, 1 = M4, 2 = Glock

    private Vector3 pauseMenuStartPosition;
    private Quaternion pauseMenuStartRotation;

    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    [SerializeField] private Transform canvasTransform;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private GameObject Crosshair;
    [SerializeField] private Transform pauseMenuTransform;

    [SerializeField] private GameObject M4_Showcase;
    [SerializeField] private GameObject M16_Showcase;
    [SerializeField] private GameObject glockShowcase;

    [SerializeField] private Button weaponSwitchLeft;
    [SerializeField] private Button weaponSwitchRight;
   
    [SerializeField] private GameObject StartGameUI;
    [SerializeField] private Text timeText;

    [SerializeField] private GameObject M4_Object;
    [SerializeField] private GameObject glock_Object;
    [SerializeField] private GameObject M16_Object;

    [SerializeField] private Button startGameButton;
    [SerializeField] private Button returnToTitlescreen;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button pauseMenuTitlescreenButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button resumeButton;

    private bool paused = false;


    bool glock = false;
    bool M4 = false;
    bool M16 = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance != null)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuStartPosition = pauseMenu.transform.position;
        pauseMenuStartRotation = pauseMenu.transform.rotation;

        anim = FindObjectOfType<Animator>();
        Crosshair.SetActive(false);
        RaycastShoot.instance.gridshotIsPlaying = true;
        isPlaying = false;
        StartGameUI.SetActive(true);
        M4_Object.SetActive(false);
        glock_Object.SetActive(true);
        M16_Object.SetActive(false);

        //Buttons
        if(weaponSwitchLeft)
        {
            weaponSwitchLeft.onClick.AddListener(LeftSwitch);
        }

        if (weaponSwitchRight)
        {
            weaponSwitchRight.onClick.AddListener(RightSwitch);
        }

        if (startGameButton)
        {
            startGameButton.onClick.AddListener(BeginGame);
        }

        if(pauseMenuTitlescreenButton)
        {
            //pauseMenuTitlescreenButton.onClick.AddListener(LoadLevel(0));
        }

        if(settingsButton)
        {
            
        }

        if (resumeButton)
        {
            resumeButton.onClick.AddListener(ResumeFromPause);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeFromPause();
        }

        anim.SetBool("GlockBool", glock);
        anim.SetBool("M4Bool", M4);
        anim.SetBool("M16Bool", M16);
        StartScreenGunDisplay(weaponShowcase, isPlaying);
        timeText.text = "TIME: " + timeLeft.ToString();
        
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;//Locks cursor at the center of the screen
            Cursor.visible = false;//Makes cursor invisable
        }

        if (weaponShowcase > 2)
            weaponShowcase = 0;
        else if (weaponShowcase < 0)
            weaponShowcase = 2;

        //Timer
        if (isPlaying && !isDecrementing)
            StartCoroutine(DecrementTime(1));
        
        //Reset UI + Time
        if (!isPlaying && timeLeft <= 0)
        {
            pauseMenu.transform.position = pauseMenuStartPosition;
            pauseMenu.transform.rotation = pauseMenuStartRotation;

            RaycastShoot.instance.gameStarted = false;

            //Enable Crosshair
            Crosshair.SetActive(false);

            //Character disabled + Gamemode Disable
            CharcterCamera.instance.enabled = false;
            isPlaying = false;

            //Set Cursor Active
            lockCursor = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            
            //Reset UI
            timeLeft = 60;
            StartGameUI.SetActive(true);
        }

        if (timeLeft <= 0)
            isPlaying = false;
            
        //Targets Spawn
        if (isPlaying && timeLeft > 0)
        {
            Crosshair.SetActive(true);
            if (targetsInScene < 5)
            {
                Gridshot.instance.GetTarget();
                targetsInScene++;
            }
        }
    }

    public void StartScreenGunDisplay(int _num, bool _playing)
    {
        if (_num == 0 && !_playing)
        {
            //Set M16 Active
            M16_Showcase.SetActive(true);
            glockShowcase.SetActive(false);
            M4_Showcase.SetActive(false);


        }
        else if (_num == 1 && !_playing)
        {
            //Set M4 Active
            M4_Showcase.SetActive(true);
            M16_Showcase.SetActive(false);
            glockShowcase.SetActive(false);
        }
        else if (_num == 2 && !_playing)
        {
            //Set Glock Active   
            glockShowcase.SetActive(true);
            M4_Showcase.SetActive(false);
            M16_Showcase.SetActive(false);
        }

    }

   

    IEnumerator DecrementTime(int _time)
    {
        isDecrementing = true;

        yield return new WaitForSecondsRealtime(_time);

        if(!paused)
        timeLeft -= 1;

        isDecrementing = false;
    }

    public void M4SetActive()
    {
        glock = true;

        RaycastShoot.instance.playM4Audio = true;
        RaycastShoot.instance.playM16Audio = false;
        RaycastShoot.instance.playGlockAudio = false;


        M16_Object.SetActive(false);
        glock_Object.SetActive(false);
        M4_Object.SetActive(true);
    }

    public void M16SetActive()
    {
        RaycastShoot.instance.playM4Audio = false;
        RaycastShoot.instance.playM16Audio = true;
        RaycastShoot.instance.playGlockAudio = false;

        glock_Object.SetActive(false);
        M4_Object.SetActive(false);
        M16_Object.SetActive(true);
    }

    public void GlockSetActive()
    {
        RaycastShoot.instance.playM4Audio = false;
        RaycastShoot.instance.playM16Audio = false;
        RaycastShoot.instance.playGlockAudio = true;

        M4_Object.SetActive(false);
        M16_Object.SetActive(false);
        glock_Object.SetActive(true);
    }

    public void BeginGame()
    {
        //Game Start
        RaycastShoot.instance.gameStarted = true;

        //Enable Mouse Movement
        CharcterCamera.instance.enabled = true;

        GunSelected(weaponShowcase);

        //Start UI Disable
        StartGameUI.SetActive(false);

        RaycastShoot.instance.accuracy = 100;

        //Reset Score + Accuracy + Time 
        RaycastShoot.instance.missed = 0;
        RaycastShoot.instance.shotsHit = 0;

        //Game Start
        isPlaying = true;
        lockCursor = true;
        glockShowcase.SetActive(false);
        M4_Showcase.SetActive(false);
        M16_Showcase.SetActive(false);
    }

    public void LeftSwitch()
    {
        weaponShowcase--;
    }

    public void RightSwitch()
    {
        weaponShowcase++;
    }

    public void GunSelected(int _weaponChosen)
    {
        if (_weaponChosen == 0)
        {
            //M16
            M16SetActive();
            anim.SetTrigger("M16Pullout");
            M16 = true;
            

        }
        else if (_weaponChosen == 1)
        {
            //M4
            M4SetActive();
            anim.SetTrigger("M4Pullout");
            M4 = true;


        }
        else if (_weaponChosen == 2)
        {
            //Glock
            GlockSetActive();
            anim.SetTrigger("GlockPullout");
            glock = true;

        }
    }

    public void ResumeFromPause()
    {
        if (!StartGameUI.activeSelf)
        {
            if (paused)
            {
                pauseMenu.SetActive(false);
                //Game Start
                RaycastShoot.instance.gameStarted = true;

                //Enable Mouse Movement
                CharcterCamera.instance.enabled = true;

                //Game Start
                lockCursor = true;

                RaycastShoot.instance.paused = false;

                paused = false;
            }
            else
            {
                //Enable UI
                pauseMenu.SetActive(true);

                //Enable Crosshair
                Crosshair.SetActive(false);

                //Character disabled + Gamemode Disable
                CharcterCamera.instance.enabled = false;

                //Set Cursor Active
                lockCursor = false;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;

                RaycastShoot.instance.paused = true;

                paused = true;
                //transform.LookAt(settingsButton.gameObject.transform.position);
            }
        }
    }
}
