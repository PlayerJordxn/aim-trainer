using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLerp : MonoBehaviour
{
    //Intro Lerp
    [SerializeField] Vector3 introStartPos;
    [SerializeField] Vector3 introEndPos;
    [SerializeField] Transform whiteBoard;

    //Game mode lerp
    [SerializeField] Vector3 modeStartLerpPos;
    [SerializeField] Vector3 modeEndLerpPos;

    //Settings Lerp


    float startTime;
    float speed = 1.4f;
    float introLength;



    private bool introCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;

        //Intro Lerp
        if (introLength <= 0)
            introLength = 1.0f;

        introLength = Vector3.Distance(introStartPos, introEndPos);
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (!introCompleted)
        {
            introCompleted = IntroLerp();
        }
        else
        {
            Debug.Log("Mode Lerping Now");
            //Mode Lerp
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




}
