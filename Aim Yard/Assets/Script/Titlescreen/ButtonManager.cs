using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public UnityEvent buttonPressed;
    [SerializeField] Camera lookFrom;
    float lookDistance;
    // Start is called before the first frame update
    void Start()
    {
        if (lookDistance <= 0)
            lookDistance = 60f;

        buttonPressed.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            buttonRaycast();
        }
    }

    public void buttonRaycast()
    {
        RaycastHit hit;

        if(Physics.Raycast(lookFrom.transform.position, lookFrom.transform.forward, out hit, lookDistance))
        {
            if(hit.collider.tag == "Start")
            {

            }

            if(hit.collider.tag == "Settings")
            {

            }

            if(hit.collider.tag == "Quit")
            {
                buttonPressed.Invoke();
            }
        }

        
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
