using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Image progressBar;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }
    

    public IEnumerator LoadScene(string _sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(_sceneName);
        scene.allowSceneActivation = false;

        loadingCanvas.SetActive(true);

        do
        {
            //Wait
            progressBar.fillAmount = scene.progress;
            yield return null;
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        loadingCanvas.SetActive(false);
        
    }
}
