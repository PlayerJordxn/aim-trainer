using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridshotSpawner : MonoBehaviour
{
    Gridshot gridshot;
    RaycastShoot raycastScript;

    public int targetsInScene = 0;
    public bool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        gridshot = FindObjectOfType<Gridshot>();
        raycastScript = FindObjectOfType<RaycastShoot>();

        raycastScript.gridshotIsPlaying = true;
        isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaying)
        {
            if(targetsInScene < 5)
            {
                gridshot.GetTarget();
                targetsInScene++;
            }
            
        }
    }


}
