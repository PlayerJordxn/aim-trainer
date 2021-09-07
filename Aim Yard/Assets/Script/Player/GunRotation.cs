using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    Rigidbody rb;
    float rotationSpeed;

    bool isDragging = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rotationSpeed <= 0)
            rotationSpeed = 10f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

   
}
