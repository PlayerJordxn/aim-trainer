using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    private int index = 0;
    float speed = 5f;
    float minSpeed = 5f;
    float maxSpeed = 10f;
    void OnEnable()
    {
        index = Random.Range(0, MovingTargetPoolManager.instance.spawnLocations.Length);
        speed = RandomSpeed();
        transform.localScale = RandomSize();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = MovingTargetPoolManager.instance.spawnLocations[index].transform.position;
        float step = Time.deltaTime * speed;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if(distance > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
        else
        {
            index = Random.Range(0, MovingTargetPoolManager.instance.spawnLocations.Length);
        }
    }

    private float RandomSpeed()
    {
        return Random.Range(minSpeed, maxSpeed);
    }

    private Vector3 RandomSize()
    {
        float randomSize = Random.Range(1, 3);
        return new Vector3(randomSize, randomSize, randomSize);
    }
}
