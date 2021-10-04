using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadshotModeSpawner : MonoBehaviour
{
    public static HeadshotModeSpawner instance;

    HeadshotMode headshotManager;
    RaycastShoot raycastScript;

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
        raycastScript = FindObjectOfType<RaycastShoot>();
        headshotManager = FindObjectOfType<HeadshotMode>();
        raycastScript.headshotModeIsPlaying = true;
        isPlaying = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaying)
        {
            if(targetsInScene < 5)
            {
                headshotManager.GetTarget();
                targetsInScene++;
            }
        }
    }
}
