using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleTargetBehaviour : MonoBehaviour
{
    private Slider healthSlider;

    private float maxHealth = 100.0f;
    public float currentHealth;
    private float minHealth = 0.0f;
    private int index = 0;
    private float speed = 0.0f;



    private void OnEnable()
    {
        //Reset health
        currentHealth = maxHealth;
        index = Random.Range(0, ScalePool.instance.spawnLocations.Length);
    }


    // Start is called before the first frame update
    void Start()
    {
        healthSlider = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= minHealth)
        {
            print("RETURNED");
            //Return Target
            ScalePool.instance.ReturnTarget(this.gameObject);
            SingleTrackingTargetManager.instance.targetCount--;
            SingleTrackingTargetManager.instance.currentTargetCount++;
        }

        healthSlider.value = currentHealth;
    }
}
