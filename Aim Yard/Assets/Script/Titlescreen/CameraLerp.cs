using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLerp : MonoBehaviour
{
    //Script access
    ButtonManager buttonManagerScript;

    //Intro Lerp
    [SerializeField] Vector3 introStartPos;
    [SerializeField] Vector3 introEndPos;
    [SerializeField] Transform whiteBoard;
    private bool introCompleted = false;

    //Game mode lerp
    [SerializeField] Vector3 modeEndLerpPos;
    [SerializeField] Transform gameModesTransform;

    //Settings Lerp
    [SerializeField] Vector3 settingsEndPos;

    //Lerp Variables
    float startTime;
    float speed = 1.4f;
    float introLength;

    //Stops intro else statement being called more than once
    int calledOnce = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Disable buttons
        buttonManagerScript = GetComponent<ButtonManager>();
        buttonManagerScript.clickable = false;

        //Lerp
        startTime = Time.time;

        if (introLength <= 0)
            introLength = 1.0f;

        introLength = Vector3.Distance(introStartPos, introEndPos);

        //
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!introCompleted)
        {
            introCompleted = IntroLerp();
        }
        else if(introCompleted && calledOnce < 1)
        {
            //Set Bool
            introCompleted = true;
            
            //Sets Buttons Clickable
            buttonManagerScript.clickable = true;

            calledOnce++;
        }


    }

    public bool IntroLerp()
    {
        float dist = (Time.time - startTime) * speed;
        float journey = dist / introLength;

        transform.position = Vector3.Lerp(introStartPos, introEndPos, journey);
        transform.LookAt(whiteBoard);

        return journey >= 1;
    }

    public bool gameModeLerp()
    {
        float dist = (Time.time - startTime) * speed;
        float journey = dist / introLength;

        transform.position = Vector3.Lerp(introEndPos, modeEndLerpPos, journey);
        transform.LookAt(gameModesTransform);

        return journey >= 1;
    }




}
