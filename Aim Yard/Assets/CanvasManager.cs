using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public TextMeshProUGUI accuracyValueText;
    public TextMeshProUGUI timeValueText;
    public TextMeshProUGUI scoreValueText;

    public float accuracy = 0f;
    public int time = 0;
    public int score = 0;
    public int totalShots = 0;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    void Start() => ResetScoreboard();

    // Update is called once per frame
    void Update()
    {
        bool divivedByZero = totalShots > 0 && score > 0;
        if (divivedByZero)
        {
            float percent = score * 100 / totalShots;
            accuracyValueText.text = percent.ToString();
        }
        else
        {
            accuracyValueText.text = "0";
        }

        if (timeValueText) timeValueText.text = time.ToString();
        if (scoreValueText) scoreValueText.text = score.ToString();
    }

    public void AddScore(int scoreAmount)
    {
        score += scoreAmount;
    }

    public void ResetScoreboard()
    {
        print("CALLED");
        time = 60;
        score = 0;
        accuracy = 0.0f;
        totalShots = 0;
    }
}
