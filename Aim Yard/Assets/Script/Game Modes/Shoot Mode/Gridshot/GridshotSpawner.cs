using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridshotSpawner : MonoBehaviour
{
    Gridshot gridshot;
    RaycastShoot raycastScript;

    public int targetsInScene = 0;
    public bool isPlaying;

    public int timeLeft;
    public bool isDecrementing = false;
    bool lockCursor = false;

    [SerializeField] GameObject StartGameUI;

    [SerializeField] GameObject M4_Object;
    [SerializeField] GameObject glock_Object;
    [SerializeField] GameObject M16_Object;

    [SerializeField] Button StartGame_Button;
    [SerializeField] Button ReturnToTitleScreen;
    [SerializeField] Button M4_Button;
    [SerializeField] Button M16_Button;
    [SerializeField] Button Glock_Button;


    // Start is called before the first frame update
    void Start()
    {
        StartGameUI.SetActive(true);
        M4_Object.SetActive(false);
        glock_Object.SetActive(false);
        M16_Object.SetActive(false);

        if(StartGame_Button)
        {
            StartGame_Button.onClick.AddListener(BeginGame);
        }

        if(M4_Button)
        {
            M4_Button.onClick.AddListener(M4SetActive);
        }

        if(M16_Button)
        {
            M16_Button.onClick.AddListener(M16SetActive);
        }

        if(Glock_Button)
        {
            Glock_Button.onClick.AddListener(GlockSetActive);
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
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;//Locks cursor at the center of the screen
            Cursor.visible = false;//Makes cursor invisable
        }

        if (isPlaying)
            StartCoroutine(DecrementTime(1));
        else if (!isPlaying)
        {
            lockCursor = false;
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
        M16_Object.SetActive(false);
        glock_Object.SetActive(false);
        M4_Object.SetActive(true);
    }

    public void M16SetActive()
    {
        glock_Object.SetActive(false);
        M4_Object.SetActive(false);
        M16_Object.SetActive(true);
    }

    public void GlockSetActive()
    {
        M4_Object.SetActive(false);
        M16_Object.SetActive(false);
        glock_Object.SetActive(true);
    }

    public void BeginGame()
    {
        timeLeft = 60;
        isPlaying = true;
        lockCursor = true;
        StartGameUI.SetActive(false);
    }


}
