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
    public bool unlimitedHealth;

    void OnAwake()
    {
        playerCurrentHealth = 100;
    }

    public void GainHealth(string tag){
        if (tag == "SmallHealth"){
            if(playerCurrentHealth < 100)
            {
                playerCurrentHealth += 5;
            }
        }
        else if (tag == "MediumHealth"){
            if(playerCurrentHealth < 100)
            {
                playerCurrentHealth += 10;
            }
        }
        else if (tag == "LargeHealth"){
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
        if (!unlimitedHealth)
        {
            if ((playerCurrentHealth > 0) && !isInvincible)
            {
                playerCurrentHealth -= damageTaken;
                StartCoroutine(dmgVFX.DamageVFX());
                StartCoroutine(camShake.Shake(0.2f, 0.35f));

                if (playerCurrentHealth <= 0)
                {
                    playerCurrentHealth = 0;

                }
                if (hasIFrames)
                {
                    StartCoroutine(IFrames());
                }
                UI.updateHealthUI(playerCurrentHealth);//call UI function
            }
        }
        /*if ((playerCurrentHealth > 0) && !isInvincible){
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
        }*/
    }

    IEnumerator IFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }

}
