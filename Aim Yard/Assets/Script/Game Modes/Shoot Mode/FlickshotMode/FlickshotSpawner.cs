using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlickshotSpawner : MonoBehaviour
{
    public static FlickshotSpawner instance;
    private Animator anim;

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

    private Vector3 spineStartPosition;
    private Quaternion spineStartRotation;

    //Transforms
    [SerializeField] private Transform spineTransform;
    [SerializeField] private GameObject Crosshair;

    //Weapon showcase before start
    [SerializeField] private GameObject M4_Showcase;
    [SerializeField] private GameObject M16_Showcase;
    [SerializeField] private GameObject glockShowcase;

    //Showcase left and right buttons
    [SerializeField] private Button weaponSwitchLeft;
    [SerializeField] private Button weaponSwitchRight;

    [SerializeField] private GameObject StartGameUI;
    [SerializeField] private Text timeText;

    [SerializeField] private GameObject M4_Object;
    [SerializeField] private GameObject glock_Object;
    [SerializeField] private GameObject M16_Object;

    //UI
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button returnToTitlescreen;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button pauseMenuTitlescreenButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button resumeButton;

    private bool paused = false;
    private bool glock = false;
    private bool M4 = false;
    private bool M16 = false;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        spineStartPosition = spineTransform.position;
        spineStartRotation = spineTransform.rotation;

        anim = FindObjectOfType<Animator>();
        Crosshair.SetActive(false);
        CharcterCamera.instance.enabled = false;
        RaycastShoot.instance.flickshotModeIsPlaying = true;
        isPlaying = false;
        StartGameUI.SetActive(true);
        M4_Object.SetActive(false);
        glock_Object.SetActive(true);
        M16_Object.SetActive(false);

        //Buttons
        if (weaponSwitchLeft)
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

        if (resumeButton)
        {
            resumeButton.onClick.AddListener(ResumeFromPause);
        }

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("GlockBool", glock);
        anim.SetBool("M4Bool", M4);
        anim.SetBool("M16Bool", M16);
        StartScreenGunDisplay(weaponShowcase, isPlaying);
        timeText.text = "TIME: " + timeLeft.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeFromPause();
        }

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;//Locks cursor at the center of the screen
            Cursor.visible = false;//Makes cursor invisable
        }

        if (timeLeft <= 0)
            isPlaying = false;

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
            spineTransform.transform.position = spineStartPosition;
            spineTransform.transform.rotation = spineStartRotation;
            RaycastShoot.instance.gameStarted = false;

            M4 = false;
            M16 = false;
            glock = false;

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

        //Targets Spawn
        if (isPlaying && timeLeft > 0)
        {
            RaycastShoot.instance.gameStarted = true;
            Crosshair.SetActive(true);
            if (targetsInScene < 5)
            {
                Flickshot.instance.GetTarget();
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

        if (!paused)
            timeLeft -= 1;

        isDecrementing = false;
    }

    private void M4SetActive()
    {

        RaycastShoot.instance.playM4Audio = true;
        RaycastShoot.instance.playM16Audio = false;
        RaycastShoot.instance.playGlockAudio = false;


        M16_Object.SetActive(false);
        glock_Object.SetActive(false);
        M4_Object.SetActive(true);
    }

    private void M16SetActive()
    {
        RaycastShoot.instance.playM4Audio = false;
        RaycastShoot.instance.playM16Audio = true;
        RaycastShoot.instance.playGlockAudio = false;

        glock_Object.SetActive(false);
        M4_Object.SetActive(false);
        M16_Object.SetActive(true);
    }

    private void GlockSetActive()
    {
        RaycastShoot.instance.playM4Audio = false;
        RaycastShoot.instance.playM16Audio = false;
        RaycastShoot.instance.playGlockAudio = true;

        M4_Object.SetActive(false);
        M16_Object.SetActive(false);
        glock_Object.SetActive(true);
    }

    private void BeginGame()
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

    private void LeftSwitch()
    {
        weaponShowcase--;
    }

    private void RightSwitch()
    {
        weaponShowcase++;
    }

    private void GunSelected(int _weaponChosen)
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

    private void ResumeFromPause()
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
                spineTransform.transform.position = spineStartPosition;
                spineTransform.transform.rotation = spineStartRotation;

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
