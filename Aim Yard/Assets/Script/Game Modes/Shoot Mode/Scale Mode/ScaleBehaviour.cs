using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBehaviour : MonoBehaviour
{
    private float speed = 0f;
    private float minSpeed = 0.5f;
    private float maxSpeed = 3f;
    private Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f);

    void OnEnable()
    {
        //Set scale and speed
        int randomSize = Random.Range(1, 4);
        Vector3 startScale = new Vector3(randomSize, randomSize, randomSize);
        transform.localScale = startScale;
        speed = Random.Range(minSpeed, maxSpeed);
        StartCoroutine(ScaleTarget());
    }

    void OnDisable()
    {
        StopCoroutine(ScaleTarget());
    }

    private IEnumerator ScaleTarget()
    {
        int id = LeanTween.scale(this.gameObject, minScale, speed).id;

        while(LeanTween.isTweening(id))
        {
            yield return null;
        }

        if(this.gameObject.activeSelf)
        {
            ScaleModeManager.instance.targetCount--;
            ScaleModePool.instance.ReturnTarget(this.gameObject);
        }
    }
}
