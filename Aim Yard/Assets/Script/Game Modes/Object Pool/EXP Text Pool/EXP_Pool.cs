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
        for(int i = 0; i < 10; i++)
        {
            //Spawn Gameobject
            GameObject expTextTemp = Instantiate(expTextPrefab, spawnLocations);

            //Add gameobject to the qeueu
            pool.Enqueue(expTextTemp);
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

            expObject.gameObject.transform.localPosition += Vector3.up;

            TextMeshProUGUI expText = GetComponent<TextMeshProUGUI>();

            expText.color = new Color(-Time.deltaTime, -Time.deltaTime, -Time.deltaTime);
            //Return
            return expObject;
        }

        return null;
    }

    public void ReturnPoolItem(GameObject _target)
    {
        pool.Enqueue(_target);
        _target.SetActive(false);
    }
}
