using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDetectMove : MonoBehaviour
{
    
    private bool isDetected;
    private Transform playerTransform;

    private float startY;
    //private float waitTime;
    //private float elapsedTime;

    public float healthDropSpeed = 7.0f;

    void Start(){
        isDetected = false;
        startY = transform.parent.transform.position.y;
        //waitTime = 2f;
        //elapsedTime = 0f;
    }

    void Update(){
        if (isDetected){
            healthDropSpeed += .01f;
            transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, playerTransform.Find("Orientation")
                                                                        .gameObject.transform.position, healthDropSpeed * Time.deltaTime);
        }

        //elapsedTime += Time.deltaTime;
        transform.parent.Rotate(0, 90f * Time.deltaTime, 0);
        transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, startY + Mathf.PingPong(Time.time * 0.5f, 0.5f), transform.parent.transform.position.z);
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Player"){
            playerTransform = col.gameObject.transform;
            isDetected = true;
        }
    }

}
