using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillhouseManager : MonoBehaviour
{
    public static KillhouseManager instance;
    private Animator anim;
    private Camera cam;

    private Vector3 spineStartPosition;
    private Quaternion spineStartRotation;

    [SerializeField] private Button resumeButton;

    [SerializeField] private Transform spineTransform;

    [SerializeField] private GameObject Crosshair;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private Text timerText;
    [SerializeField] private Text targetsRemaingText;

    [SerializeField] private AudioClip glockSfxClip;
    [SerializeField] private AudioSource glockSfx;

    [SerializeField] private AudioClip TargetSfxClip;
    [SerializeField] private AudioSource TargetSfx;

    [SerializeField] private GameObject headshotEmblem;
    [SerializeField] private GameObject bodyShotEmblem;

    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject glock;

    //Final Statisitics
    [SerializeField] private Text timeText;
    [SerializeField] private Text headshotsText;
    [SerializeField] private Text targetsText;
    [SerializeField] private Text accuracyText;

    private bool paused = false;
    private bool lockCursor = false;


    private bool knifeEquipped = true;
    private bool canSwapWeapon = true;

    private int bulletsHit = 0;
    private int bulletsMissed = 0;
    private int bulletTotal = 0;

    private bool isDecrementing = false;
    public float timer = 0;
    public int targetsHit = 0;
    public int headshotHits = 0;

    public float finalTimeCompletion = 0;
    public int finalTargetsHit = 0;
    public int finalHeadshotCount = 0;

    

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
        lockCursor = true;
        spineStartPosition = spineTransform.position;
        spineStartRotation = spineTransform.rotation;

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
        //Final Statistics Wall
        timeText.text = "TIME: " + finalTimeCompletion.ToString();
        headshotsText.text = "HEADSHOTS: " + finalHeadshotCount.ToString();
        targetsText.text = "TARGET COUNT: " + finalTargetsHit.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeFromPause();
        }

        if (bulletsHit > 0 && bulletsMissed > 0)
        {
            float percent = (bulletsHit / bulletsMissed) * 100f;
            float round = Mathf.Round(percent);
            //double round = System.Math.Round(percent, 2);
            Debug.Log("Percent: " + percent );
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

        if(timer <= 0)
            PlayerController.instance.isPlaying = false;

        if (PlayerController.instance.isPlaying)
            timer -= Time.deltaTime;
        else
            timer = 60f;


        //Shoot
        if (Input.GetKeyDown(KeyCode.Mouse0) && canSwapWeapon && glock.activeSelf && !PlayerController.instance.isRunning)
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
        if (Input.GetKeyDown(KeyCode.Q))
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
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 50f))
        {
            bulletTotal++;
            
            if(hit.collider.tag == "Body")
            {
                TargetSfx.PlayOneShot(TargetSfxClip);
                targetsHit++;
                StartCoroutine(EnableKillemblem(0.5f, bodyShotEmblem));
                hit.collider.gameObject.transform.parent.gameObject.SetActive(false);
                bulletsHit++;
            }

            if (hit.collider.tag == "Head")
            {
                TargetSfx.PlayOneShot(TargetSfxClip);
                targetsHit++;
                StartCoroutine(EnableKillemblem(0.5f, headshotEmblem));
                hit.collider.gameObject.transform.parent.gameObject.SetActive(false);
                headshotHits++;
                bulletsHit++;
            }

            if (hit.collider.tag == "Neck")
            {
                TargetSfx.PlayOneShot(TargetSfxClip);
                targetsHit++;
                StartCoroutine(EnableKillemblem(0.5f, bodyShotEmblem));
                hit.collider.gameObject.transform.parent.gameObject.SetActive(false);
                bulletsHit++;
            }

            
            if(hit.collider.tag != "Head" && hit.collider.tag != "Neck" && hit.collider.tag != "Body")
            {
                bulletsMissed++;
            }

            Debug.Log(bulletsMissed);
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
        }
    }

}
