using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image loadingImage;


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
        if (loadingScreen)
            loadingScreen.SetActive(false);
    }


    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    //Button Methods
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        if(loadingScreen)
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingImage.fillAmount = progress;

            yield return null;
        }
      
    }

    IEnumerator UnloadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneIndex);

        if (loadingScreen)
            loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingImage.fillAmount = progress;

            yield return null;
        }

    }


}
