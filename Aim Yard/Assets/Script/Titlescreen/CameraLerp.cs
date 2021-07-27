using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    //Intro Lerp
    [SerializeField] Vector3 introStartPos;
    [SerializeField] Vector3 introEndPos;
    float startTime;
    float speed = 1.0f;
    float introLength;
    

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
    void Update()
    {
        IntroLerp();
    }

    public void IntroLerp()
    {
        float dist = (Time.time - startTime) * speed;
        float journey = dist / introLength;

        transform.position = Vector3.Lerp(introStartPos, introEndPos, journey);
    }

    
}
