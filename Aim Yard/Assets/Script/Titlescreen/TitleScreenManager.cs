using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    
    [SerializeField] private Button playButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button settingsButton;

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private GameObject playUI;
    [SerializeField] private GameObject homeUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject headerUI;

    // Start is called before the first frame update
    void Start()
    {

        homeButton.onClick.AddListener(delegate { SwitchUIElements();});
        playButton.onClick.AddListener(delegate { SwitchUIElements(playUI);});
        settingsButton.onClick.AddListener(delegate { SwitchUIElements(settingsUI);});

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SwitchUIElements(GameObject exceptionElement=null)
    {
        var childrenCount = mainCanvas.transform.childCount;
        for (var idx = 0; idx < childrenCount; idx++)
        {
            var child = mainCanvas.transform.GetChild(idx);
            if (exceptionElement && child.name == exceptionElement.name)
            {
                exceptionElement.SetActive(true);
            }
            else if (child.name != headerUI.name)
            {
                child.gameObject.SetActive(false);
            }

        }
    }
}
