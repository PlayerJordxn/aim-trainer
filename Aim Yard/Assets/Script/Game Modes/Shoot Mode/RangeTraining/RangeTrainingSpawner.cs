using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTrainingSpawner : MonoBehaviour
{
    //Script Access
    RangeTrainingMode rangeTrainingManager;
    RaycastShoot raycastScript;
    


    bool isPlaying;
    public int targetsInScene = 0;

    // Start is called before the first frame update
    void Start()
    {
        rangeTrainingManager = FindObjectOfType<RangeTrainingMode>();
        raycastScript = FindObjectOfType<RaycastShoot>();


        raycastScript.rangeTrainingIsPlaying = true;
        isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            if (targetsInScene < 1)
            {
                rangeTrainingManager.GetTarget();

                targetsInScene++;
            }
        }
    }
}
