using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTrackingBehaviour : MonoBehaviour
{
    private Vector3 resetSize;
    private float speed = 0.0f;
    private float maxSpeed = 4.0f;
    private float minSpeed = 0.5f;
    private float minScale = 0.4f;
    private int index = 0;
    void OnEnable()
    {
        int scale = Random.Range(1, 3);
        speed = Random.Range(minSpeed, maxSpeed);
        resetSize = new Vector3(scale,scale,scale);
        transform.localScale = resetSize;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        if(currentScale.x < minScale && currentScale.y < minScale && currentScale.z < minScale)
        {
            ScalePool.instance.ReturnTarget(this.gameObject);
            ScaleTrackingManager.instance.targetCount--;
            ScaleTrackingManager.instance.currentTargetCount++;
        }

        float step = Time.deltaTime * speed;
        float distance = Vector3.Distance(transform.position, ScalePool.instance.spawnLocations[index].position);

        if (distance <= 0.001f)
        {
            index = Random.Range(0, ScalePool.instance.spawnLocations.Length);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, ScalePool.instance.spawnLocations[index].position, step);
        }
    }
}
