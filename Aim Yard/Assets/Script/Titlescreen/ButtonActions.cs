using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    [SerializeField] CameraLerp cameraScript;
    GameObject[] ButtonModes;
    bool completed;

    private void Start()
    {
        cameraScript = GameObject.FindObjectOfType<CameraLerp>();

        //Set inactive buttons
        /*
        for(int i = 0; i < ButtonModes.Length; i++)
        {
            GameObject temp = Instantiate(ButtonModes[i]);//Add Pos
        }
        */
    }

    public void ChooseMode()
    {
        if(!completed)
        {
            completed = cameraScript.gameModeLerp();
        }
        else
        {
            Debug.Log("Completed");
        }
    }

    public void ChangeSettings()
    {
        Debug.Log("Change Settings");
    }

    public void EndGame()
    {
        Debug.Log("End Game");
    }
}
