using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickshotSpawner : MonoBehaviour
{
    public static FlickshotSpawner instance;

    RaycastShoot raycastScript;
    Flickshot flickshotManager;

    bool isPlaying;

    public int targetsInScene = 0;

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

    // Start is called before the first frame update
    void Start()
    {
        flickshotManager = FindObjectOfType<Flickshot>();
        raycastScript = FindObjectOfType<RaycastShoot>();

        isPlaying = true;
        raycastScript.flickshotModeIsPlaying = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaying)
        {
            if(targetsInScene < 1)
            {
                flickshotManager.GetTarget();
                targetsInScene++;
            }
        }
    }
}
