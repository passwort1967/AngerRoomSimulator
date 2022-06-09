using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoGoController : MonoBehaviour
{
    public GameObject midBodyOffset;
    public GameObject handInteractor;
    public float nonLiniarStart;
    public float nonLiniarScaleFactor;
    




    // Update is called once per frame
    void Update()
    {
        //Distance betweeen Reference and Controller
        float distance = Vector3.Distance(midBodyOffset.transform.position, gameObject.transform.position);
        if (distance <= nonLiniarStart)
        {
            handInteractor.transform.localPosition = Vector3.zero;
            return;
        }

        Vector3 direction = (midBodyOffset.transform.position - gameObject.transform.position).normalized;


        float scaleFactor = (nonLiniarStart - distance) * nonLiniarScaleFactor;
        handInteractor.transform.position = gameObject.transform.position + (direction * scaleFactor);
    }
}


   
