using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [SerializeField] private float length = 0f;
    [SerializeField] private float speed = 0f;

    [SerializeField] private bool xMovement = false;
    [SerializeField] private bool zMovement = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(xMovement)
        transform.position = new Vector3(Mathf.PingPong(Time.time * speed, length), transform.position.y, transform.position.z);

        if(zMovement)
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(Time.time * speed, length));

    }
}
