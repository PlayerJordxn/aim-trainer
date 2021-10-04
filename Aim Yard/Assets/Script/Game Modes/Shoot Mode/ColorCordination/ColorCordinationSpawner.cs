using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCordinationSpawner : MonoBehaviour
{
    public static ColorCordinationSpawner instance;

    //Script Access
    ColorCordination colorCordination;
    RaycastShoot raycastScript;

    //Targets in scene
    public int targetsInScene = 0;

    //bool check
    public bool isPlaying;

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

    private void Start()
    {
        colorCordination = FindObjectOfType<ColorCordination>();
        raycastScript = FindObjectOfType<RaycastShoot>();

        raycastScript.colorCordinationIsPlaying = true;
        isPlaying = true;
    }



    private void Update()
    {
        if (isPlaying)
        {
            if (targetsInScene < 5)
            {
                colorCordination.GetTarget();
                targetsInScene++;
            }

        }
    }
}
