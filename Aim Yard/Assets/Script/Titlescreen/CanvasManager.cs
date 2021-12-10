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


  

    // Start is called before the first frame update
    void Start()
    {
        if (loadingScreen)
            loadingScreen.SetActive(false);
    }


    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(1);
        //StartCoroutine(LoadScene());
        //StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void UnloadLevel(int sceneIndex)
    {
        
        StartCoroutine(LoadScene());
    }

    //Button Methods
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);


        if(loadingScreen)
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if(loadingImage)
            loadingImage.fillAmount = progress;

            yield return null;
        }
    }

    IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

        while(!operation.isDone)
        {
            yield return null;
        }
    }

    IEnumerator UnloadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneIndex, UnloadSceneOptions.None);

        if (loadingScreen)
            loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if(loadingImage)
            loadingImage.fillAmount = progress;

            yield return null;
        }

    }


}
