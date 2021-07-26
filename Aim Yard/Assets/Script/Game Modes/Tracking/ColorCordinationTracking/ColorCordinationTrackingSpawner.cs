using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCordinationTrackingSpawner : MonoBehaviour
{
    //Script Access
    ColorCordinationTracking colorCordinationTracking;
    RaycastShoot raycastScript;
    TargetBehavior targetBehaviour;


    bool isPlaying;
    public int targetsInScene = 0;

    // Start is called before the first frame update
    void Start()
    {
        colorCordinationTracking = FindObjectOfType<ColorCordinationTracking>();
        raycastScript = FindObjectOfType<RaycastShoot>();


        raycastScript.colorCordinationTrackingIsPlaying = true;
        isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            if (targetsInScene < 1)
            {
                colorCordinationTracking.GetTarget();
                targetsInScene++;
            }
        }
    }
}
