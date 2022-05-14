using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitlescreenManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup titlescreenGroup;
    [SerializeField] private CanvasGroup modesGroup;

    [SerializeField] private Button playButton;


    // Start is called before the first frame update
    void Start()
    {
        if (playButton)
        {
            playButton.onClick.AddListener(delegate { SwitchCanvas(titlescreenGroup, modesGroup); });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SwitchCanvas(CanvasGroup currentCanvas, CanvasGroup newCanvas)
    {
        float speed = 0.3f;
        float maxAplha = 1f;
        float minAlpha = 0f;
        currentCanvas.alpha = Mathf.Lerp(currentCanvas.alpha, minAlpha, speed);
        newCanvas.alpha = Mathf.Lerp(newCanvas.alpha, maxAplha, speed);

        while (newCanvas.alpha > minAlpha && currentCanvas.alpha < maxAplha)
        {
            yield return null;
        }
    }
}
