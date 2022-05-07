using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TargetHealth : MonoBehaviour
{
    private int index = 0;
    private int speed = 0;
    private int minSpeed = 1;
    private int maxSpeed = 12;
    private void OnEnable()
    {
        float minScale = 1f;
        float maxScale = 3f;
        float scaleSize = Random.Range(minScale, maxScale);
        Vector3 newScale = new Vector3(scaleSize, scaleSize, scaleSize);
        transform.localScale = newScale;

        speed = Random.Range(minSpeed, maxSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        //Return target to pool
        Vector3 currentScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        float minScale = 0.4f;
        if(currentScale.x <= minScale && currentScale.y <= minScale && currentScale.z <= minScale)
        {
            ColourCordinationTrackingPool.instance.ReturnTarget(this.gameObject);
            ColourCordinationTrackingManager.instance.activeTargetCount--;
            ColourCordinationTrackingManager.instance.targetsDestroyed++;
        }

        Move();
    }

    private void Move()
    {
        float step = Time.deltaTime * speed;
        float distance = Vector3.Distance(transform.position, ColourCordinationTrackingPool.instance.spawnLocations[index].position);

        if (distance <= 0.001f)
        {
            index = Random.Range(0, ColourCordinationTrackingPool.instance.spawnLocations.Length);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, ColourCordinationTrackingPool.instance.spawnLocations[index].position, step);
        }
    }
}
