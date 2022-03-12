using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EXP_Pool : MonoBehaviour
{
    public static EXP_Pool instance;

    //Target Prefab
    [SerializeField] GameObject expTextPrefab;

    //Transform array
    public Transform spawnLocations;

    //Queue
    public Queue<GameObject> pool = new Queue<GameObject>();

    //Exp text parent
    public Canvas scoreCanvas;

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
        
        for(int i = 0; i < 15; i++)
        {
            //Spawn Gameobject
            GameObject expTextTemp = Instantiate(expTextPrefab, spawnLocations);
            //Set parent to canvas
            expTextTemp.transform.parent = scoreCanvas.gameObject.transform;
            //Set start pos
            expTextTemp.GetComponent<RectTransform>().localPosition = spawnLocations.transform.localPosition;
            //Add gameobject to the qeueu
            pool.Enqueue(expTextTemp);
            //Disable
            expTextTemp.SetActive(false);
        }
        


    }

    public GameObject GetPoolItem()
    {
        if (pool.Count > 0)
        {
            //Removes from queue
            GameObject expObject = pool.Dequeue();

            //Set gameobject active
            expObject.SetActive(true);

            //Return
            return expObject;
        }

        return null;
    }

    public void ReturnPoolItem(GameObject _target)
    {
        //Add to queue
        pool.Enqueue(_target);
        //Reset to start pos
        _target.transform.localPosition = spawnLocations.transform.localPosition;
        //Disable
        _target.SetActive(false);
    }
}
