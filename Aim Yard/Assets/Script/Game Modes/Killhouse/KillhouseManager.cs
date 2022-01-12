using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillhouseManager : MonoBehaviour
{
    public static KillhouseManager instance;
    private Animator anim;
    private Camera cam;

    [SerializeField] private Button resumeButton;

    [SerializeField] private Transform spineTransform;

    [SerializeField] private GameObject Crosshair;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private Text timerText;
    [SerializeField] private Text targetsRemaingText;

    [SerializeField] private AudioClip glockSfxClip;
    [SerializeField] private AudioSource glockSfx;

    [SerializeField] private AudioClip TargetHeadshotSfxClip;
    [SerializeField] private AudioSource TargetHeadshotSfx;

    [SerializeField] private AudioClip TargetHitSfxClip;
    [SerializeField] private AudioSource TargetHitSfx;

    [SerializeField] private GameObject headshotEmblem;
    [SerializeField] private GameObject bodyShotEmblem;

    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject glock;

    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Text sensitivityTextValue;


    //Final Statisitics
    [SerializeField] private Text timeText;
    [SerializeField] private Text headshotsText;
    [SerializeField] private Text targetsText;
    [SerializeField] private Text accuracyText;

    private bool paused = false;
    private bool lockCursor = false;


    private bool knifeEquipped = true;
    private bool canSwapWeapon = true;

    private float bulletsHit = 0;
    private float bulletsMissed = 0;
    private int bulletTotal = 0;
    public double accuracy = 0f;

    private bool isDecrementing = false;
    public float timer = 0;
    public int targetsHit = 0;
    public int headshotHits = 0;

    public float finalTimeCompletion = 0f;
    public int finalTargetsHit = 0;
    public int finalHeadshotCount = 0;
    public double finalAccuracy = 0f;

    

    void Awake()
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
        sensitivitySlider.value = 5;
        PlayerController.instance.cameraRotationSpeed = 5f;
        lockCursor = true;
        cam = FindObjectOfType<Camera>();
        anim = FindObjectOfType<Animator>();
        glock.SetActive(false);
        knife.SetActive(true);

        if (resumeButton)
        {
            resumeButton.onClick.AddListener(ResumeFromPause);
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController.instance.cameraRotationSpeed = sensitivitySlider.value * 50;
        sensitivityTextValue.text = sensitivitySlider.value.ToString();

        //Final Statistics Wall
        timeText.text = "TIME: " + finalTimeCompletion.ToString();
        headshotsText.text = "HEADSHOTS: " + finalHeadshotCount.ToString();
        targetsText.text = "TARGET COUNT: " + finalTargetsHit.ToString() + " / " + " 15";
        accuracyText.text = "ACCURACY: " + accuracy + "%";

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeFromPause();
        }

        if (bulletsHit > 0 && bulletsMissed > 0)
        {
            float percent = (bulletsHit / bulletsMissed) * 100f;
            double round = System.Math.Round(percent, 2);
            accuracy = round;
            Debug.Log("Hit: " + bulletsHit + " --- Missed: " + bulletsMissed + " --- Percent: " + round);
        }

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;//Locks cursor at the center of the screen
            Cursor.visible = false;//Makes cursor invisable
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;//Locks cursor at the center of the screen
            Cursor.visible = true;//Makes cursor invisable

        }



        //Camera UI
        float timerRound = (float)System.Math.Round(timer, 2);
        timerText.text = "TIME: " + timerRound.ToString();
        targetsRemaingText.text = "Target Count: " + targetsHit.ToString() + " / 15";

        //Animation
        anim.SetBool("isRunning", PlayerController.instance.isRunning);
        WeaponSwitch();

        if(!paused)
        {
            if (timer <= 0)
                PlayerController.instance.isPlaying = false;

            if (PlayerController.instance.isPlaying)
                timer -= Time.deltaTime;
            else
                timer = 60f;
        }
        


        //Shoot
        if (Input.GetKeyDown(KeyCode.Mouse0) && canSwapWeapon && glock.activeSelf && !PlayerController.instance.isRunning &&!paused)
        {
            glockSfx.PlayOneShot(glockSfxClip);
            Shoot();
        }
    }

    private IEnumerator WeaponDelayEnable(float _wait, GameObject _currentWeapon, GameObject _disableWeapon)
    {
        _disableWeapon.SetActive(false);
        yield return new WaitForSecondsRealtime(_wait);
        _currentWeapon.SetActive(true);
        
    }

    private IEnumerator WeaponChangeDelay()
    {
        canSwapWeapon = false;
        yield return new WaitForSecondsRealtime(.6f);
        canSwapWeapon = true;
    }

    private void WeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !paused)
        {
            if (knifeEquipped)
            {
                if (canSwapWeapon)
                {
                    knifeEquipped = false;
                    anim.SetBool("KnifeEquipped", true);
                    StartCoroutine(WeaponDelayEnable(0.3f, glock, knife));
                    anim.SetTrigger("KnifeEnable");

                    if (canSwapWeapon)
                        StartCoroutine(WeaponChangeDelay());
                }
            }
            else
            {
                if (canSwapWeapon)
                {
                    PlayerController.instance.isRunning = false;
                    knifeEquipped = true;
                    anim.SetBool("KnifeEquipped", false);
                    StartCoroutine(WeaponDelayEnable(0.3f, knife, glock));

                    if (canSwapWeapon)
                        StartCoroutine(WeaponChangeDelay());
                }
            }
        }
    }

    private void Shoot()
    {
        bulletsMissed++;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 50f))
        {
            bulletTotal++;
            
            if(hit.collider.tag == "Body")
            {
                TargetHeadshotSfx.PlayOneShot(TargetHeadshotSfxClip);
                targetsHit++;
                StartCoroutine(EnableKillemblem(0.5f, bodyShotEmblem));
                hit.collider.gameObject.transform.parent.gameObject.SetActive(false);
                bulletsHit++;
            }

            if (hit.collider.tag == "Head")
            {
                
                TargetHeadshotSfx.PlayOneShot(TargetHeadshotSfxClip);
                targetsHit++;
                StartCoroutine(EnableKillemblem(0.5f, headshotEmblem));
                hit.collider.gameObject.transform.parent.gameObject.SetActive(false);
                headshotHits++;
                bulletsHit++;
            }

            if (hit.collider.tag == "Neck")
            {
                
                TargetHeadshotSfx.PlayOneShot(TargetHeadshotSfxClip);
                targetsHit++;
                StartCoroutine(EnableKillemblem(0.5f, bodyShotEmblem));
                hit.collider.gameObject.transform.parent.gameObject.SetActive(false);
                bulletsHit++;
            }

            if(hit.collider.tag == "Wall")
            {
                TargetHitSfx.PlayOneShot(TargetHitSfxClip);
            }


            if (hit.collider.tag != "Head" && hit.collider.tag != "Neck" && hit.collider.tag != "Body")
            {
               
            }

            
        }      
    }

    private IEnumerator EnableKillemblem(float _wait, GameObject _image)
    {
        _image.SetActive(true);
        yield return new WaitForSecondsRealtime(_wait);
        _image.SetActive(false);
    }

    private void ResumeFromPause()
    {
        if (paused)
        {
            pauseMenu.SetActive(false);


            //Enable Mouse Movement
            PlayerController.instance.enabled = true;

            //Game Start
            lockCursor = true;

            paused = false;
        }
        else
        {

            //Enable UI
            pauseMenu.SetActive(true);

            //Enable Crosshair
            Crosshair.SetActive(false);

            //Character disabled + Gamemode Disable
            PlayerController.instance.enabled = false;

            //Set Cursor Active
            lockCursor = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;


            paused = true;
        }
    }

}
