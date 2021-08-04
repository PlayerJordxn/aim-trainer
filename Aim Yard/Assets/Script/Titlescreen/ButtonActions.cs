using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
  
    GameObject[] ButtonModes;

    private void Start()
    {
       

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
