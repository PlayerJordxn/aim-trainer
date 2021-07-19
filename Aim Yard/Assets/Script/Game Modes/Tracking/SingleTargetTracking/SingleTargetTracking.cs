using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetTracking : MonoBehaviour
{
    [SerializeField] GameObject trackingTarget;

    GameObject[] transformAsGameobjects;

    Transform[] spawnLocations = new Transform[25];

    Queue<GameObject> pool = new Queue<GameObject>();


    private void Start()
    {
        //Gathers all spawn locations
        transformAsGameobjects = GameObject.FindGameObjectsWithTag("SpawnLocation");

        for (int t = 0; t < spawnLocations.Length; t++)
        {
            //Converts spawn locations from gameobject to transform
            spawnLocations[t] = transformAsGameobjects[t].transform;
        }

        GameObject temp = Instantiate(trackingTarget);

        pool.Enqueue(temp);

        temp.SetActive(false);
    }

    public GameObject GetTarget()
    {
        //Gameojbect
        GameObject target = pool.Dequeue();

        //Random Spawn
        RandomSpawnPosition(target);
        
        //Set Active
        target.SetActive(true);

        //Return
        return target;
    }

    public void ReturnTarget(GameObject _target)
    {
        //Enqueue
        pool.Enqueue(_target);

        //Set Inactive
        _target.SetActive(false);
            
    }

    public void RandomSpawnPosition(GameObject _target)
    {
        int random = Random.Range(0, spawnLocations.Length);
        _target.transform.position = spawnLocations[random].transform.position;
    }    
}
