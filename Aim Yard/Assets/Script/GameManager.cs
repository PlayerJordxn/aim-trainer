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
      
    }

    

    public void QuitGame()
    {
        Application.Quit();
    }
}
