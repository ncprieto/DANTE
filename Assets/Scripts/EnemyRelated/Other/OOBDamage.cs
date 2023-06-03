using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOBDamage : MonoBehaviour
{
    public PlayerHealth playerHP;

    private float dotTimer;
    private float expCount;

    private bool inTrigger;

    void Awake()
    {
        playerHP = GameObject.Find("Player").GetComponent<PlayerHealth>();
        dotTimer = 0;
        expCount = .01f;
    }

    void Update(){
        if (!inTrigger){
            dotTimer += Time.deltaTime;
            if (dotTimer > (.25f - expCount)){
                playerHP.ReceiveDamage(2, false);
                dotTimer = 0;
                expCount *= 1.035f;
                if (expCount > .25f){
                    expCount = .24f;
                }
            }   
        }
        else{
            dotTimer = 0;
            expCount = .01f;
        }
        inTrigger = false;
    }

    void OnTriggerStay(Collider col){
        if (col.gameObject.tag == "Player"){
            inTrigger = true;
        }
    }
}
