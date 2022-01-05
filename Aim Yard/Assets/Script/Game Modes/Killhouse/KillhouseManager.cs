using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillhouseManager : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private AudioClip glockSfxClip;
    [SerializeField] private AudioSource glockSfx;


    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject glock;

    private bool knifeEquipped = true;
    private bool canSwapWeapon = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = FindObjectOfType<Animator>();
        glock.SetActive(false);
        knife.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isRunning", PlayerController.instance.isRunning);

        if(Input.GetKeyDown(KeyCode.Mouse0) && canSwapWeapon && glock.activeSelf && !PlayerController.instance.isRunning)
        {
            glockSfx.PlayOneShot(glockSfxClip);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(knifeEquipped)
            {
                if(canSwapWeapon)
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
                if(canSwapWeapon)
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

    IEnumerator WeaponDelayEnable(float _wait, GameObject _currentWeapon, GameObject _disableWeapon)
    {
        _disableWeapon.SetActive(false);
        yield return new WaitForSecondsRealtime(_wait);
        _currentWeapon.SetActive(true);
        
    }

    IEnumerator WeaponChangeDelay()
    {
        canSwapWeapon = false;
        yield return new WaitForSecondsRealtime(.6f);
        canSwapWeapon = true;
    }
}
