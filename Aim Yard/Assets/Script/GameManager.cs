using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private CanvasGroup titlescreenGroup;
    [SerializeField] private CanvasGroup modesGroup;

    //Titlescreen
    public Button playButton;
    public Button optionsButton;
    public Button customizationButton;
    public Button exitGame;

    //Play screen Buttons

    //Options screen buttons

    //Customization screen buttons

    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance != null)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (playButton)
        {
            playButton.onClick.AddListener(delegate { SwitchCanvas(titlescreenGroup, modesGroup); });
        }

        //if (optionsButton)
        //{
        //    optionsButton.onClick.AddListener(delegate { SwitchCanvas(); });
        //}

        //if (customizationButton)
        //{
        //    optionsButton.onClick.AddListener(delegate { SwitchCanvas(); });
        //}

        //if(exitGame)
        //{
        //    exitGame.onClick.AddListener(delegate { SwitchCanvas(); });
        //}
    }

    public IEnumerator SwitchCanvas(CanvasGroup currentCanvas, CanvasGroup newCanvas)
    {
        float speed = 0.3f;
        float maxAplha = 1f;
        float minAlpha = 0f;
        currentCanvas.alpha = Mathf.Lerp(currentCanvas.alpha, minAlpha, speed);
        newCanvas.alpha = Mathf.Lerp(newCanvas.alpha, maxAplha, speed);

        while(newCanvas.alpha > minAlpha && currentCanvas.alpha < maxAplha)
        {
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
