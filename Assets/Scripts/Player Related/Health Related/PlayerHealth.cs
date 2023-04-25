using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public bool isInvincible;
    public int playerCurrentHealth = 100;
    private float dotTimer;
    public UI_Script UI;            //To call function in UI

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = 50;
        
        dotTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("HEALTH: " + playerCurrentHealth);
        //UI.updateHealthUI(playerCurrentHealth); //Call function in UI
    }

    // Health pickups
    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "SmallHealth"){
            Destroy(col.gameObject);
            if (playerCurrentHealth < 100){
                playerCurrentHealth += 5;
                UI.updateHealthUI(playerCurrentHealth);//call UI function
                if (playerCurrentHealth >= 100){
                    playerCurrentHealth = 100;
                }
            }
        }
        else if (col.gameObject.tag == "MediumHealth"){
            Destroy(col.gameObject);
            if (playerCurrentHealth < 100){
                playerCurrentHealth += 10;
                UI.updateHealthUI(playerCurrentHealth);//call UI function
                if (playerCurrentHealth >= 100){
                    playerCurrentHealth = 100;
                }
            }
        }
        else if (col.gameObject.tag == "LargeHealth"){
            Destroy(col.gameObject);
            if (playerCurrentHealth < 100){
                playerCurrentHealth += 25;
                UI.updateHealthUI(playerCurrentHealth);//call UI function
                if (playerCurrentHealth >= 100){
                    playerCurrentHealth = 100;
                }
            }
        }
    }

    // Called by enemy scripts
    public void ReceiveDamage(int damageTaken, bool hasIFrames){
        if ((playerCurrentHealth > 0) && !isInvincible){
            playerCurrentHealth -= damageTaken;
            UI.updateHealthUI(playerCurrentHealth);//call UI function
            if (playerCurrentHealth <= 0){
                playerCurrentHealth = 0;
            }
            if (hasIFrames){
                StartCoroutine(IFrames());
            }
        }
    }

    IEnumerator IFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(3f);
        isInvincible = false;
    }

}
