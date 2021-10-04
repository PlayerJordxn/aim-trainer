using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickshot : MonoBehaviour
{
    public static Flickshot instance;

    //Target Prefab
    [SerializeField] GameObject targetPrefab;

    //Gameobject Array
    GameObject[] transformAsGameobjects;

    //Transform array
    public Transform[] spawnLocations = new Transform[25];

    //Queue
    public Queue<GameObject> pool = new Queue<GameObject>();

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

        //Gathers all spawn locations
        transformAsGameobjects = GameObject.FindGameObjectsWithTag("SpawnLocation");

        for (int t = 0; t < spawnLocations.Length; t++)
        {
            //Converts spawn locations from gameobject to transform
            spawnLocations[t] = transformAsGameobjects[t].transform;
        }

        int random = Random.Range(0, spawnLocations.Length);

        //Spawn Gameobject
        GameObject targetTemp = Instantiate(targetPrefab, spawnLocations[random]);

        //Add gameobject to the queue
        pool.Enqueue(targetTemp);

        //Set gameobject false
        targetTemp.SetActive(false);

        
    }

    public GameObject GetTarget()
    {
        if (pool.Count > 0)
        {
            //Random int
            int random = Random.Range(0, spawnLocations.Length);

            //Removes from queue
            GameObject target = pool.Dequeue();

            //Random spawn
            target.transform.position = spawnLocations[random].transform.position;

            //Random scale
            RandomScale(target);

            //Set gameobject active
            target.SetActive(true);

            //Return
            return target;
        }

        return null;
    }

    public void ReturnTarget(GameObject _target)
    {
        pool.Enqueue(_target);
        _target.SetActive(false);
    }

    public void RandomScale(GameObject _gameobject)
    {
        int random = Random.Range(1, 3);
        _gameobject.transform.localScale = new Vector3(random, random, random);
    }
}
