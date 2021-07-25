using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTrackingTargetSpawner : MonoBehaviour
{
    //Script Access
    SingleTargetTracking singleTargetTrackingManager;
    RaycastShoot raycastScript;
    TargetBehavior targetBehaviour;


    bool isPlaying;
    public int targetsInScene = 0;

    // Start is called before the first frame update
    void Start()
    {
        singleTargetTrackingManager = FindObjectOfType<SingleTargetTracking>();
        raycastScript = FindObjectOfType<RaycastShoot>();


        raycastScript.singleTargetTrackingIsPlaying = true;
        isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaying)
        {
            if(targetsInScene < 1)
            {
                singleTargetTrackingManager.GetTarget();
                targetsInScene++;
            }
        }
    }
}
