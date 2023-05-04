using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public bool isInvincible;
    public int playerCurrentHealth = 100;
    public UI_Script UI;            //To call function in UI
    public DamageVignette dmgVFX;
    public CameraShake camShake;

    void OnAwake()
    {
        playerCurrentHealth = 100;
    }

    // Health pickups
    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "SmallHealth"){
            Destroy(col.gameObject);
            if(playerCurrentHealth < 100)
            {
                playerCurrentHealth += 5;
            }
        }
        else if (col.gameObject.tag == "MediumHealth"){
            Destroy(col.gameObject);
            if(playerCurrentHealth < 100)
            {
                playerCurrentHealth += 10;
            }
        }
        else if (col.gameObject.tag == "LargeHealth"){
            Destroy(col.gameObject);
            if(playerCurrentHealth < 100)
            {
                playerCurrentHealth += 25;
            }
        }
        playerCurrentHealth = Mathf.Clamp(playerCurrentHealth, 0, 100);
        UI.updateHealthUI(playerCurrentHealth);//call UI function
    }

    // Called by enemy scripts
    public void ReceiveDamage(int damageTaken, bool hasIFrames){
        if ((playerCurrentHealth > 0) && !isInvincible){
            playerCurrentHealth -= damageTaken;
            StartCoroutine(dmgVFX.DamageVFX());
            StartCoroutine(camShake.Shake(0.2f, 0.35f));
            
            if (playerCurrentHealth <= 0){
                playerCurrentHealth = 0;
                
            }
            if (hasIFrames){
                StartCoroutine(IFrames());
            } 
           UI.updateHealthUI(playerCurrentHealth);//call UI function
        }
    }

    IEnumerator IFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }

}
