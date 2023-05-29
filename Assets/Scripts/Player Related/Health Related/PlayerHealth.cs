using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Variables")]
    public int playerCurrentHealth = 100;
    public bool isInvincible;
    public bool unlimitedHealth;

    [Header("VFX")]
    public DamageVignette dmgVFX;
    public CameraShake camShake;
    private bool canHealVFX;

    [Header("UI Elements")]
    public  GameObject UICanvas;
    public  GameObject HealthBarPrefab;
    public  GameObject BackgroundPrefab;
    private GameObject HealthBar;
    private GameObject Background;
    private BarAndNumber HPBarScript;

    void OnAwake()
    {
        playerCurrentHealth = 100;
    }

    void Start()
    {
        canHealVFX  = true;
        Background  = Instantiate(BackgroundPrefab, UICanvas.transform, false);
        HealthBar   = Instantiate(HealthBarPrefab,  UICanvas.transform, false);
        HPBarScript = HealthBar.GetComponent<BarAndNumber>();
        HPBarScript.SetSliderAndNumber(playerCurrentHealth);
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
        
        if(Input.GetKeyDown(KeyCode.H)) SetHealthTo(50);
        if(Input.GetKeyDown(KeyCode.J)) EnableUI();
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
        HPBarScript.SetSliderAndNumber(playerCurrentHealth);
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
            HPBarScript.SetSliderAndNumber(playerCurrentHealth);
        }
    }

    public void SetHealthTo(int amount)
    {
        playerCurrentHealth = amount;
        HPBarScript.SetSliderAndNumber(amount);
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

    public void EnableUI()
    {
        HealthBar.SetActive(true);
        Background.SetActive(true);
    }

    public void DisableUI()
    {
        HealthBar.SetActive(false);
        Background.SetActive(false);
    }
}
