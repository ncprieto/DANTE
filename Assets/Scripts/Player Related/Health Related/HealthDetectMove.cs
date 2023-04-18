using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDetectMove : MonoBehaviour
{
    
    private bool isDetected;
    private Transform playerTransform;

    public float healthDropSpeed = 7.0f;

    void Start(){
        isDetected = false;
    }

    void Update(){
        if (isDetected){
            healthDropSpeed += .01f;
            transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, playerTransform.Find("Orientation")
                                                                        .gameObject.transform.position, healthDropSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Player"){
            playerTransform = col.gameObject.transform;
            isDetected = true;
        }
    }

}
