using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCordination : MonoBehaviour
{
    //Target Prefab
    [SerializeField] GameObject targetPrefab;

    //Material 
    [SerializeField] Material[] material = new Material[8];

    //Gameobject Array
    GameObject[] transformAsGameobjects;

    //Transform array
    public Transform[] spawnLocations = new Transform[25];

    //Queue
    public Queue<GameObject> pool = new Queue<GameObject>();

    //List
    List<int> storage = new List<int>();

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

        for (int e = 0; e < spawnLocations.Length; e++)
        {
            //Random transform
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
                e--;

        }
    }

    public GameObject GetTarget()
    {
        if (pool.Count > 0)
        {
            //Random
            int random = Random.Range(0, spawnLocations.Length);

            //Removes from queue
            GameObject target = pool.Dequeue();

            //Assign random material
            ChangeMaterial(target);

            //Assign random scale of object
            RandomScale(target);

            //Random spawn location
            if(!storage.Contains(random))
                target.transform.position = spawnLocations[random].transform.position;

            //Set gameobject active
            target.SetActive(true);

            //Return
            return target;
        }

        return null;
    }

    //Disable target
    public void ReturnTarget(GameObject _target)
    {
        pool.Enqueue(_target);
        _target.SetActive(false);
    }

    //Change material
    public void ChangeMaterial(GameObject _gameobject)
    {
        _gameobject.GetComponent<MeshRenderer>().material = RandomMaterial();
    }

    //Random material colour
    public Material RandomMaterial()
    {
        int random = Random.Range(0, material.Length);
        Material randomMaterial = material[random];
        return randomMaterial;
    }

    //Random target size
    public void RandomScale(GameObject _gameobject)
    {
        int random = Random.Range(1, 3);
        _gameobject.transform.localScale = new Vector3(random, random, random);
    }
}
