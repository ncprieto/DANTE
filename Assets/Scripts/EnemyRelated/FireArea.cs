using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArea : MonoBehaviour
{
    public PlayerHealth playerHP;

    private float dotTimer;

    void Awake()
    {
        playerHP = GameObject.Find("Player").GetComponent<PlayerHealth>();
        dotTimer = 0;
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
