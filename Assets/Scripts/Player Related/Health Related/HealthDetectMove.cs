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
    private float time;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isMovingUp;

    void Start(){
        isDetected = false;
        startY = transform.parent.transform.position.y;
        //waitTime = 2f;
        //elapsedTime = 0f;
        startPos = transform.parent.transform.position;
        endPos = new Vector3( startPos.x, startPos.y + .5f, startPos.z);
        time = 0;
        isMovingUp = true;
    }

    void Update(){
        if (isDetected){
            healthDropSpeed += .01f;
            transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, playerTransform.Find("Orientation")
                                                                        .gameObject.transform.position, healthDropSpeed * Time.deltaTime);
        }
        transform.parent.Rotate(0, 90f * Time.deltaTime, 0);

        time += Time.deltaTime;
        if (isMovingUp)
        {
            transform.parent.transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, time / 1));
            if (transform.parent.transform.position.y >= endPos.y)
            {
                time = 0;
                isMovingUp = false;
            }
            //StartCoroutine(Moveheart(startPos, endPos));
        }else
        {
            transform.parent.transform.position = Vector3.Lerp(endPos, startPos, Mathf.SmoothStep(0, 1, time / 1));
            if (transform.parent.transform.position.y <= startPos.y)
            {
                time = 0;
                isMovingUp = true;
            }
            //StartCoroutine(Moveheart(endPos, startPos));
        }

        //elapsedTime += Time.deltaTime;

        //transform.parent.transform.position = new Vector3(transform.parent.transform.position.x, startY + Mathf.PingPong(Time.time * 0.5f, 0.5f), transform.parent.transform.position.z);
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Player"){
            playerTransform = col.gameObject.transform;
            isDetected = true;
        }
    }
}
