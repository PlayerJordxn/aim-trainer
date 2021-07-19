using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    //Script Access
    SingleTargetTracking singleTargetManager;
    SingleTrackingTargetSpawner singleTargetSpawnerScript;

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

        isRemovingHealth = false;

        if (currentHealth <= 0)
            currentHealth = maxHealth;

        if (currentHealth <= 0)
            currentHealth = 100f;

        if (removeHealthAmount <= 0)
            removeHealthAmount = 8f;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveHealth(GameObject _target)
    {
        Debug.Log(currentHealth);

        if(currentHealth <= minHealth)
        {
            //Reset Health
            //currentHealth = maxHealth;

            //Enqeue
            singleTargetManager.ReturnTarget(_target);

            //Remove target in scene
            singleTargetSpawnerScript.targetsInScene--;
        }
        
        if(!isRemovingHealth)
        StartCoroutine(healthReduction(0.4f));
    }

    

    IEnumerator healthReduction(float wait)
    {
        isRemovingHealth = true;
        currentHealth -= removeHealthAmount;
        yield return new WaitForSeconds(wait);
        isRemovingHealth = false;
    }

    
   
}
