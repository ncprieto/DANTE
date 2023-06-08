using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Variables")]
    public int playerCurrentHealth = 100;
    public bool isInvincible;
    public bool unlimitedHealth;
    public bool isTutorial;
    private int previousHealth = 100;

    [Header("VFX")]
    public DamageVignette dmgVFX;
    public CameraShake camShake;
    private bool canHealVFX;

    [Header("UI Elements")]
    public  GameObject UICanvas;
    public  GameObject HealthBarPrefab;
    private GameObject HealthBar;
    private BarAndNumber HPBarScript;

    [Header("SFX")]
    public string gainHealthSFX;
    public string takeDamageSFX;
    
    private float sfxVolume;
    private FMOD.Studio.EventInstance gainHealthSFXEvent;
    private FMOD.Studio.EventInstance takeDamageSFXEvent;

    void OnAwake()
    {
        playerCurrentHealth = 100;
    }

    void Start()
    {
        canHealVFX  = true;
        HealthBar   = Instantiate(HealthBarPrefab,  UICanvas.transform, false);
        HPBarScript = HealthBar.GetComponent<BarAndNumber>();
        HPBarScript.SetSliderAndNumber(playerCurrentHealth);
        if (isTutorial) StartCoroutine(WaitForVignette());
        SetUpAudio();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha8)) unlimitedHealth = true;
        
        if (previousHealth > 25 && playerCurrentHealth <= 25){
            dmgVFX.enterLowHP = true;
        }
        else if (previousHealth <= 25 && playerCurrentHealth > 25){
            dmgVFX.exitLowHP = true;
        }

        if (playerCurrentHealth <= 25){
            dmgVFX.isLowHP = true;
        }
        else{
            dmgVFX.isLowHP = false;
        }
        previousHealth = playerCurrentHealth;
    } 

    public void GainHealth(string tag){
        if(playerCurrentHealth < 100)
        {
            if      (tag == "SmallHealth")  playerCurrentHealth += 5;
            else if (tag == "MediumHealth") playerCurrentHealth += 10;
            else if (tag == "LargeHealth")  playerCurrentHealth += 25;
            gainHealthSFXEvent.start();
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
            takeDamageSFXEvent.start();
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

    private void SetUpAudio()
    {
        sfxVolume = PlayerPrefs.GetFloat("Master", 0.75f) * PlayerPrefs.GetFloat("SFX", 1f);
        gainHealthSFXEvent = RuntimeManager.CreateInstance(gainHealthSFX);
        takeDamageSFXEvent = RuntimeManager.CreateInstance(takeDamageSFX);
        gainHealthSFXEvent.setVolume(sfxVolume);
        takeDamageSFXEvent.setVolume(sfxVolume);
    }

    public void EnableUI()
    {
        HealthBar.SetActive(true);
    }

    public void DisableUI()
    {
        HealthBar.SetActive(false);
    }

    IEnumerator WaitForVignette()
    {
        yield return new WaitForSeconds(.1f);
        playerCurrentHealth = 25;
        HPBarScript.SetSliderAndNumber(playerCurrentHealth);
    }
}
