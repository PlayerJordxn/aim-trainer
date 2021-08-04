using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    public UnityEvent buttonPressed;
    float lookDistance;
    public bool clickable;

    // Start is called before the first frame update
    void Start()
    {

        cam = GetComponent<Camera>();

        if (lookDistance <= 0)
            lookDistance = 60f;


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && clickable)
        {
            clickable = false;

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, lookDistance))
            {
                buttonPressed.Invoke();
            }
        }
    }
}
