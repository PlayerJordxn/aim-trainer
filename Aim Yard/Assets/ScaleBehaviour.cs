using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBehaviour : MonoBehaviour
{
    private float speed = 0f;
    private float minSpeed = 0.5f;
    private float maxSpeed = 3.5f;

    private Vector3 scaleChange = new Vector3(0.1f, 0.1f, 0.1f);

    void OnEnable()
    {
        transform.localScale = Vector3.one;
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x > 0.001f && transform.localScale.y > 0.001f)
        {
            transform.localScale = transform.localScale - scaleChange;
        }
        else
        {
            ScaleModeManager.instance.targetCount--;
            ScaleModePool.instance.ReturnTarget(this.gameObject);
        }
    }
}
