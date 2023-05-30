using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDetectMove : MonoBehaviour
{
    
    private bool isDetected;
    private Transform playerTransform;

    private float startY;

    public float healthDropSpeed = 9.0f;

    void Start(){
        isDetected = false;
        startY = transform.parent.transform.position.y;
    }

    void Update(){
        if (isDetected){
            healthDropSpeed += .01f;
            transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, playerTransform.Find("Orientation")
                                                                        .gameObject.transform.position, healthDropSpeed * Time.deltaTime);
        }
        else{
            transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, startY + Mathf.PingPong(Time.time * 0.5f, 0.5f), transform.parent.transform.position.z);
        }
        transform.parent.Rotate(0, 90f * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Player"){
            playerTransform = col.gameObject.transform;
            isDetected = true;
        }
    }

}
