using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridshotSpawner : MonoBehaviour
{
    public static GridshotSpawner instance;
    Animator anim;

    public int targetsInScene = 0;
    public bool isPlaying;

    public int timeLeft;
    public bool isDecrementing = false;
    bool lockCursor = false;

    bool rifleActive = false;
    int weaponShowcase = 0;

    //Rotating Weapons
    [SerializeField] GameObject M4_Showcase;
    [SerializeField] GameObject M16_Showcase;
    [SerializeField] GameObject glockShowcase;

    [SerializeField] Button weaponSwitchLeft;
    [SerializeField] Button weaponSwitchRight;

    [SerializeField] GameObject scoreUI;
 
    [SerializeField] GameObject StartGameUI;
    [SerializeField] TextMeshProUGUI timeText;

    [SerializeField] GameObject M4_Object;
    [SerializeField] GameObject glock_Object;
    [SerializeField] GameObject M16_Object;

    [SerializeField] Button StartGame_Button;
    [SerializeField] Button ReturnToTitleScreen;

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
        
        anim = FindObjectOfType<Animator>();
        scoreUI.SetActive(false);
        CharcterCamera.instance.enabled = false;
        rifleActive = false;

        StartGameUI.SetActive(true);
        M4_Object.SetActive(false);
        glock_Object.SetActive(true);
        M16_Object.SetActive(false);

        if(weaponSwitchLeft)
        {
            weaponSwitchLeft.onClick.AddListener(LeftSwitch);
        }

        if (weaponSwitchRight)
        {
            weaponSwitchRight.onClick.AddListener(RightSwitch);
        }

        if (StartGame_Button)
        {
            StartGame_Button.onClick.AddListener(BeginGame);
        }

        RaycastShoot.instance.gridshotIsPlaying = true;
        isPlaying = false;

        if (timeLeft <= 0)
            timeLeft = 60;
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = timeLeft.ToString();
        anim.SetBool("Rifle", rifleActive);

        StartScreenGunDisplay(weaponShowcase, isPlaying);

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;//Locks cursor at the center of the screen
            Cursor.visible = false;//Makes cursor invisable
        }

        if (isPlaying && !isDecrementing)
            StartCoroutine(DecrementTime(1));
        else if (!isPlaying)
            StartGameUI.SetActive(true);


        if (isPlaying && timeLeft > 0)
        {
            if (targetsInScene < 5)
            {
                Gridshot.instance.GetTarget();
                targetsInScene++;
            }
        }
        
        if(timeLeft <= 0)
        {
            //Character disabled
            CharcterCamera.instance.enabled = false;
            isPlaying = false;

            //Deactivate UI
            StartGameUI.SetActive(true);

            //Decativate Current Weapon Objects
            M4_Object.SetActive(false);
            M16_Object.SetActive(false);
            glock_Object.SetActive(false);

            //Enable UI
            scoreUI.SetActive(false);

            //Set Cursor Active
            lockCursor = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            StartGameUI.SetActive(true);
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
        timeLeft -= 1;
        isDecrementing = false;
    }

    public void M4SetActive()
    {
        RaycastShoot.instance.m4a1SfxBool = true;
        RaycastShoot.instance.m16SfxBool = false;
        RaycastShoot.instance.glockSfxBool = false;


        M16_Object.SetActive(false);
        glock_Object.SetActive(false);
        M4_Object.SetActive(true);
    }

    public void M16SetActive()
    {
        RaycastShoot.instance.m4a1SfxBool = false;
        RaycastShoot.instance.m16SfxBool = true;
        RaycastShoot.instance.glockSfxBool = false;

        glock_Object.SetActive(false);
        M4_Object.SetActive(false);
        M16_Object.SetActive(true);
    }

    public void GlockSetActive()
    {
        RaycastShoot.instance.m4a1SfxBool = false;
        RaycastShoot.instance.m16SfxBool = false;
        RaycastShoot.instance.glockSfxBool = true;

        M4_Object.SetActive(false);
        M16_Object.SetActive(false);
        glock_Object.SetActive(true);
    }

    public void BeginGame()
    {
        GunSelected(weaponShowcase, rifleActive);

        //Disable Weapon Showcase
        M4_Showcase.SetActive(false);
        M16_Showcase.SetActive(false);
        glockShowcase.SetActive(false);
        
        //Enable Mouse Movement
        CharcterCamera.instance.enabled = true;

        //Deactivate UI
        StartGameUI.SetActive(false);

        //Enable UI
        scoreUI.SetActive(true);

        //Reset Score + Accuracy + Time 
        RaycastShoot.instance.shotsFired = 0;
        RaycastShoot.instance.shotsHit = 0;
        timeLeft = 60;

        //Game Start
        isPlaying = true;
        lockCursor = true;
    }

    public void LeftSwitch()
    {
        weaponShowcase--;
    }

    public void RightSwitch()
    {
        weaponShowcase++;
    }

    public void GunSelected(int _weaponChosen, bool activateRifle)
    {
        if (_weaponChosen == 0)
        {
            //M16
            M16SetActive();
            activateRifle = true;
        }
        else if (_weaponChosen == 1)
        {
            //M4
            M4SetActive();
            activateRifle = true;
        }
        else if (_weaponChosen == 2)
        {
            //Glock
            GlockSetActive();
            activateRifle = false;
        }
    }

}
