using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TargetHealth : MonoBehaviour
{
    public float currentHealth = 100f;
    private float maxHealth = 100f;
    private Slider healthBar;
    private int index = 0;
    private int speed = 0;
    private int minSpeed = 1;
    private int maxSpeed = 15;
    private void OnEnable()
    {
        currentHealth = maxHealth;
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        healthBar.value = currentHealth;
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
