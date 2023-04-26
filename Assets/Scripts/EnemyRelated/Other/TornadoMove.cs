using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoMove : MonoBehaviour
{

    public GameObject player;
    public PlayerHealth playerHP;

    public float moveSpeed;
    public float spinSpeed;

    private float dotTimer;

    void Awake()
    {
        player = GameObject.Find("Player");
        playerHP = player.GetComponent<PlayerHealth>();
        dotTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {  
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, player.transform.position, moveSpeed * Time.deltaTime);
        transform.parent.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

    void OnTriggerStay(Collider col){
        if (col.gameObject.tag == "Player"){
            dotTimer += Time.deltaTime;
            if (dotTimer > .2){
                playerHP.ReceiveDamage(5, false);
                dotTimer = 0;
            }
        }
    }
}
