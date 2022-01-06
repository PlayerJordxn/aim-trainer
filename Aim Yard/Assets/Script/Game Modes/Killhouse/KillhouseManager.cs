using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillhouseManager : MonoBehaviour
{
    public static KillhouseManager instance;
    private Animator anim;
    private Camera cam;

    [SerializeField] private AudioClip glockSfxClip;
    [SerializeField] private AudioSource glockSfx;

    [SerializeField] private AudioClip TargetSfxClip;
    [SerializeField] private AudioSource TargetSfx;



    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject glock;

    private bool knifeEquipped = true;
    private bool canSwapWeapon = true;

    private bool isDecrementing = false;
    public float timer = 0;
    public float timeCompletion = 0;

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
        cam = FindObjectOfType<Camera>();
        anim = FindObjectOfType<Animator>();
        glock.SetActive(false);
        knife.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isRunning", PlayerController.instance.isRunning);
        WeaponSwitch();

        //Shoot
        if(Input.GetKeyDown(KeyCode.Mouse0) && canSwapWeapon && glock.activeSelf && !PlayerController.instance.isRunning)
        {
            glockSfx.PlayOneShot(glockSfxClip);
            Shoot();
        }

        if(PlayerController.instance.isPlaying)
        {
            if(!isDecrementing)
            StartCoroutine(Timer(1f));
        }
        else
        {

        }

        Debug.Log("Time Left: " + timer);

       
        
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

    private IEnumerator Timer(float _wait)
    {
        isDecrementing = true;
        yield return new WaitForSecondsRealtime(_wait);
        timer--;
        isDecrementing = false;
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
        if (Physics.Raycast(cam.transform.position, transform.TransformDirection(Vector3.forward), out hit, 50f))
        {
            Debug.Log("Gun Fired");

            if(hit.collider.tag == "TargetBody")
            {
                TargetSfx.PlayOneShot(TargetSfxClip);
            }

            if (hit.collider.tag == "TargetHead")
            {
                TargetSfx.PlayOneShot(TargetSfxClip);

            }

            if (hit.collider.tag == "TargetNeck")
            {
                TargetSfx.PlayOneShot(TargetSfxClip);

            }
        }
    }
   


}
