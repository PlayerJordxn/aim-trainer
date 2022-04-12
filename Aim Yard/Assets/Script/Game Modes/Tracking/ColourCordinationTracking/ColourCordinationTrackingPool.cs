using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourCordinationTrackingPool : MonoBehaviour
{
    public static ColourCordinationTrackingPool instance;

    //Target Prefab
    [SerializeField] GameObject targetPrefab;

    [SerializeField] Material[] materials;

    //Gameobject Array
    GameObject[] transformAsGameobjects;

    //Transform array
    public Transform[] spawnLocations;

    //Queue
    public Queue<GameObject> pool = new Queue<GameObject>();

    //List
    List<int> storage = new List<int>();

    private void Awake()
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
        spawnLocations = new Transform[20];

        //Gathers all spawn locations
        transformAsGameobjects = GameObject.FindGameObjectsWithTag("SpawnLocation");

        for (int t = 0; t < spawnLocations.Length; t++)
        {
            //Converts spawn locations from gameobject to transform
            spawnLocations[t] = transformAsGameobjects[t].transform;
        }

        for (int e = 0; e < spawnLocations.Length; e++)
        {
            int random = Random.Range(0, spawnLocations.Length);

            if (!storage.Contains(random))
            {
                //Spawn Gameobject
                GameObject targetTemp = Instantiate(targetPrefab, spawnLocations[random]);

                //Add to storage
                storage.Add(random);

                //Add gameobject to the qeueu
                pool.Enqueue(targetTemp);

                //Set gameobject false
                targetTemp.SetActive(false);
            }
            else
            {
                e--;
            }
        }
    }

    public GameObject GetTarget()
    {
        if (pool.Count > 0)
        {
            //Random
            int randomTransform = Random.Range(0, spawnLocations.Length);
            int randomMaterial = Random.Range(0, materials.Length);
            //Removes from queue
            GameObject target = pool.Dequeue();

            if (!storage.Contains(randomTransform))
                target.transform.position = spawnLocations[randomTransform].transform.position;
            //Random material
            target.GetComponent<MeshRenderer>().material = materials[randomMaterial];

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
}
