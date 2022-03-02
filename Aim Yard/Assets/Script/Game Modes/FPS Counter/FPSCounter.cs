using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fpsText;
    [SerializeField] private float _hudRefreshRate = 1f;

    private float _timer;

    private void Update()
    {
        if(_fpsText)
        {
            if (Time.unscaledTime > _timer)
            {
                int fps = (int)(1f / Time.unscaledDeltaTime);
                _fpsText.text = "FPS: " + fps;
                _timer = Time.unscaledTime + _hudRefreshRate;
            }
        }
    }
}
