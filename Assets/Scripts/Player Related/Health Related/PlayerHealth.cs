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

    private bool canHealVFX;

    void OnAwake()
    {
        playerCurrentHealth = 100;
    }

    void Start()
    {
        canHealVFX = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha8)) unlimitedHealth = true;

        if (playerCurrentHealth <= 25){
            dmgVFX.isLowHP = true;
        }
        else{
            dmgVFX.isLowHP = false;
        }
    }

    public void GainHealth(string tag){
        if(playerCurrentHealth < 100)
        {
            if      (tag == "SmallHealth")  playerCurrentHealth += 5;
            else if (tag == "MediumHealth") playerCurrentHealth += 10;
            else if (tag == "LargeHealth")  playerCurrentHealth += 25;
            if (canHealVFX) StartCoroutine(HealVFXTimer());
        }
        playerCurrentHealth = Mathf.Clamp(playerCurrentHealth, 0, 100);
        UI.updateHealthUI(playerCurrentHealth);//call UI function
    }

    // Called by enemy scripts
    public void ReceiveDamage(float damageTaken, bool hasIFrames){
        if(unlimitedHealth) return;
        if ((playerCurrentHealth > 0) && !isInvincible)
        {
            playerCurrentHealth -= (int)(damageTaken * PlayerPrefs.GetFloat("Incoming Damage", 1));
            StartCoroutine(dmgVFX.DamageVFX());
            StartCoroutine(camShake.Shake(0.2f, 0.35f));
            if (playerCurrentHealth <= 0) playerCurrentHealth = 0;
            if (hasIFrames) StartCoroutine(IFrames());
            UI.updateHealthUI(playerCurrentHealth); //call UI function
        }
    }

    IEnumerator IFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }

    IEnumerator HealVFXTimer()
    {
        canHealVFX = false;
        StartCoroutine(dmgVFX.HealVFX());
        yield return new WaitForSeconds(.25f);
        canHealVFX = true;
    }

}
