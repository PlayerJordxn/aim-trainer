using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridshotSpawner : MonoBehaviour
{
    public static GridshotSpawner instance;

    Gridshot gridshot;
    RaycastShoot raycastScript;

    public int targetsInScene = 0;
    public bool isPlaying;

    public int timeLeft;
    public bool isDecrementing = false;
    bool lockCursor = false;

    //Rotating Weapons
    [SerializeField] GameObject M4_Showcase;
    [SerializeField] GameObject M16_Showcase;
    [SerializeField] GameObject glockShowcase;

    [SerializeField] Button weaponSwitchLeft;
    [SerializeField] Button weaponSwitchRight;

    int weaponShowcase = 0;


    [SerializeField] GameObject codeSprite;
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
        scoreUI.SetActive(false);
        CharcterCamera.instance.enabled = false;

        StartGameUI.SetActive(true);
        M4_Object.SetActive(false);
        glock_Object.SetActive(false);
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

        

        gridshot = FindObjectOfType<Gridshot>();
        raycastScript = FindObjectOfType<RaycastShoot>();

        raycastScript.gridshotIsPlaying = true;
        isPlaying = false;

        if (timeLeft <= 0)
            timeLeft = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if(weaponShowcase == 0 && !isPlaying)
        {
            //Set M16 Active
            M16_Showcase.SetActive(true);
            glockShowcase.SetActive(false);
            M4_Showcase.SetActive(false);
            
        }
        else if(weaponShowcase == 1 && !isPlaying)
        {
            //Set M4 Active
            M4_Showcase.SetActive(true);
            M16_Showcase.SetActive(false);
            glockShowcase.SetActive(false);
            
        }
        else if(weaponShowcase == 2 && !isPlaying)
        {
            //Set Glock Active   
            glockShowcase.SetActive(true);
            M4_Showcase.SetActive(false);
            M16_Showcase.SetActive(false);
            
        }
        else if(weaponShowcase > 2)
        {
            weaponShowcase = 0;
        }
        else if(weaponShowcase < 0)
        {
            weaponShowcase = 2;
        }
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;//Locks cursor at the center of the screen
            Cursor.visible = false;//Makes cursor invisable
        }

        if (isPlaying && !isDecrementing)
            StartCoroutine(DecrementTime(1));
        else if (!isPlaying)
        {
            //lockCursor = false;
            StartGameUI.SetActive(true);
        }


        if (isPlaying && timeLeft > 0)
        {
            if (targetsInScene < 5)
            {
                gridshot.GetTarget();
                targetsInScene++;
            }
        }
        else if(timeLeft <= 0)
        {
            CharcterCamera.instance.enabled = false;
            isPlaying = false;

            //Deactivate UI
            codeSprite.SetActive(true);
            StartGameUI.SetActive(true);

            //Decativate Current Weapon Objects
            M4_Object.SetActive(false);
            M16_Object.SetActive(false);
            glock_Object.SetActive(false);

            //Enable UI
            scoreUI.SetActive(false);

            lockCursor = false;
            Cursor.lockState = CursorLockMode.Confined;//Locks cursor at the center of the screen
            Cursor.visible = true;//Makes cursor invisable

            StartGameUI.SetActive(true);
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
        raycastScript.m4a1SfxBool = true;
        raycastScript.m16SfxBool = false;
        raycastScript.glockSfxBool = false;


        M16_Object.SetActive(false);
        glock_Object.SetActive(false);
        M4_Object.SetActive(true);
    }

    public void M16SetActive()
    {
        raycastScript.m4a1SfxBool = false;
        raycastScript.m16SfxBool = true;
        raycastScript.glockSfxBool = false;

        glock_Object.SetActive(false);
        M4_Object.SetActive(false);
        M16_Object.SetActive(true);
    }

    public void GlockSetActive()
    {
        raycastScript.m4a1SfxBool = false;
        raycastScript.m16SfxBool = false;
        raycastScript.glockSfxBool = true;

        M4_Object.SetActive(false);
        M16_Object.SetActive(false);
        glock_Object.SetActive(true);
    }

    public void BeginGame()
    {
        if (weaponShowcase == 0)
        {
            //M16
            M16_Object.SetActive(true);
            glock_Object.SetActive(false);
            M4_Object.SetActive(false);
        }
        else if (weaponShowcase == 1)
        {
            //M4
            M4_Object.SetActive(true);
            M16_Object.SetActive(false);
            glock_Object.SetActive(false);
            
        }
        else if (weaponShowcase == 2)
        {
            //Glock
            glock_Object.SetActive(true);
            M4_Object.SetActive(false);
            M16_Object.SetActive(false);
            
        }

        //Disable Weapon Showcase
        M4_Showcase.SetActive(false);
        M16_Showcase.SetActive(false);
        glockShowcase.SetActive(false);
        
        //Enable Mouse Movement
        CharcterCamera.instance.enabled = true;

        //Deactivate UI
        codeSprite.SetActive(false);
        StartGameUI.SetActive(false);

        //Enable UI
        scoreUI.SetActive(true);

        //Reset Score + Accuracy + Time 
        raycastScript.shotsFired = 0;
        raycastScript.shotsHit = 0;
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
}
