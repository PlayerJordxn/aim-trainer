using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuParent;
    // Start is called before the first frame update
    void Start()
    {
        if (mainMenuParent) mainMenuParent.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLeveL(int _sceneIndex)
    {
        SceneManager.LoadScene(_sceneIndex, LoadSceneMode.Single);
    }
}
