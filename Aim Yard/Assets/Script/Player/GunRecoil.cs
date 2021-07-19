using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public Vector3 recoil;
    Vector3 normalRotation;
    public float lerpPercent;
    bool bulletShot;
    // Start is called before the first frame update
    void Start()
    {
        normalRotation = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetButtonDown("Fire1"))
            transform.localEulerAngles += recoil;
        else if(Input.GetButtonUp("Fire1"))
            transform.localEulerAngles = normalRotation;

    }

    
}
