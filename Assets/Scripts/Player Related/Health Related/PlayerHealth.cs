using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int playerCurrentHealth = 100;
    private float dotTimer;

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = 50;
        dotTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("HEALTH: " + playerCurrentHealth);
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "SmallHealth"){
            Destroy(col.gameObject);
            if (playerCurrentHealth < 100){
                playerCurrentHealth += 5;
                if (playerCurrentHealth >= 100){
                    playerCurrentHealth = 100;
                }
            }
        }
        else if (col.gameObject.tag == "MediumHealth"){
            Destroy(col.gameObject);
            if (playerCurrentHealth < 100){
                playerCurrentHealth += 10;
                if (playerCurrentHealth >= 100){
                    playerCurrentHealth = 100;
                }
            }
        }
        else if (col.gameObject.tag == "LargeHealth"){
            Destroy(col.gameObject);
            if (playerCurrentHealth < 100){
                playerCurrentHealth += 25;
                if (playerCurrentHealth >= 100){
                    playerCurrentHealth = 100;
                }
            }
        }
    }

    void OnTriggerStay(Collider col){
        if (col.gameObject.tag == "FireDOT"){
            dotTimer += Time.deltaTime;
            if ((dotTimer > .2) && (playerCurrentHealth > 0)){
                dotTimer = 0;
                playerCurrentHealth -= 5;
                if (playerCurrentHealth <= 0){
                    playerCurrentHealth = 0;
                }
            }
        }
    }

}
