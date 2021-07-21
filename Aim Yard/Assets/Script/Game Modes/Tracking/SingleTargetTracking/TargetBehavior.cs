using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetBehavior : MonoBehaviour
{
    //Script Access
    SingleTargetTracking singleTargetManager;
    SingleTrackingTargetSpawner singleTargetSpawnerScript;

    [SerializeField] Image healthBar;

    //Health
    float currentHealth;
    float maxHealth;
    float minHealth = 0;
    float removeHealthAmount;
    bool isRemovingHealth;

    // Start is called before the first frame update
    void Start()
    {
        singleTargetSpawnerScript = FindObjectOfType<SingleTrackingTargetSpawner>();
        singleTargetManager = FindObjectOfType<SingleTargetTracking>();

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void RemoveHealth(GameObject _target)
    {
        Debug.Log(currentHealth);

        if(currentHealth <= minHealth)
        {
            //Reset Health
            currentHealth = maxHealth;
            Debug.Log("Max Health: " + maxHealth);

            //Enqeue
            singleTargetManager.ReturnTarget(_target);

            //Remove target in scene
            singleTargetSpawnerScript.targetsInScene--;
        }
        else if (!isRemovingHealth)
            StartCoroutine(healthReduction(0.4f));
         
      
    }

    internal void Initialize()
    {
        isRemovingHealth = false;

        if (maxHealth <= 0)
            maxHealth = 100f;

        if (currentHealth <= 0)
            currentHealth = maxHealth;

        if (removeHealthAmount <= 0)
            removeHealthAmount = 8f;
    }

    IEnumerator healthReduction(float wait)
    {
        isRemovingHealth = true;
        currentHealth -= removeHealthAmount;
        yield return new WaitForSeconds(wait);
        isRemovingHealth = false;
    }

    
   
}
