using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTrackingBehavior : MonoBehaviour
{
   void OnEnable()
    {
        float scale = Random.Range(1.2f, 1.5f);
        transform.localScale = new Vector3();
    }

    void Update()
    {
        Vector3 currentScale = transform.localScale;
        float minScale = 0.2f;

        if(currentScale.x < minScale && currentScale.y < minScale)
        {
            ColourCordinationTrackingPool.instance.ReturnTarget(this.gameObject);
        }
    }
}
