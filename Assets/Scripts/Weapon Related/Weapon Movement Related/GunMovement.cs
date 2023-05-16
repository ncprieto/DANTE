using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class GunMovement : MonoBehaviour
{
    [Header ("Base Class Variables")]
    public KeyCode abilityKey;
    public float abilityCooldown;
    public float actualAbilityCooldown;
    public bool  IsToggleable;
    public float refundForKill;
    public string sfxKey;
    public string offCDSFXKey;

    // Player Related
    protected GameObject player;
    protected Rigidbody rb;
    protected Camera playerCamera;
    protected Movement movement;

    // Weapon Related
    protected GunAttributes gunAttributes;
    protected FOVVFX fovVFX;

    // Sound Related
    protected FMOD.Studio.EventInstance sfxEvent;
    protected FMOD.Studio.EventInstance offCDSFXEvent;
    protected BGMController bgmController;

    // Ability Start enum
    protected ABILITY abilityState;
    protected enum ABILITY
    {
        ACTIVE,
        ONCOOLDOWN,
        OFFCOOLDOWN
    }

    protected virtual void OnDestroy()
    {
        sfxEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    protected virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.Minus)) abilityCooldown = 0f; // dev tool give no cooldown to weapon ability
    }
    
    public void Initialize(GameObject playerObj, GunAttributes ga, GameObject soundSystem)
    {
        // player related
        player   = playerObj;
        rb       = player.GetComponent<Rigidbody>();
        movement = player.GetComponent<Movement>();
        // weapon related
        gunAttributes = ga;
        // fov
        fovVFX   = player.GetComponent<FOVVFX>();
        // ability stuff
        abilityState  = ABILITY.OFFCOOLDOWN;
        abilityKey    = (KeyCode)PlayerPrefs.GetInt("Weapon Ability", 304);     // get ability key from player prefs
        // sfx and bgm stuff
        sfxEvent = RuntimeManager.CreateInstance(sfxKey);
        offCDSFXEvent = RuntimeManager.CreateInstance(offCDSFXKey);
        bgmController = soundSystem.GetComponent<BGMController>();
    }

    protected virtual void DoMovementAbility()
    {
        abilityState = ABILITY.ACTIVE;
        if(!IsToggleable) StartCoroutine(StartAbilityCooldown());                             // if ability is one time use aka not toggleable start cooldown                                                                 // do sfx
    }

    protected virtual void EndMovementAbility()
    {
        if(IsToggleable) StartCoroutine(StartAbilityCooldown());                              // if ability is toggled start coold down after it ends
    }

    protected bool CanActivateAbility()
    {
        if(abilityState == ABILITY.ACTIVE || abilityState == ABILITY.ONCOOLDOWN) return false;
        return true;
    }

    protected bool CanDeactivateAbility()
    {
        if(IsToggleable && abilityState == ABILITY.ACTIVE) return true;
        return false;
    }

    IEnumerator StartAbilityCooldown()
    {
        abilityState = ABILITY.ONCOOLDOWN;
        float timeLeft = abilityCooldown;
        while(timeLeft > 0f)
        {
            if(refundFactor > 0f)
            {
                timeLeft -= refundFactor;
                refundFactor = 0f;
            }
            timeLeft -= Time.deltaTime;
            actualAbilityCooldown = timeLeft < 0f ? 0f : timeLeft;
            yield return null;
        }
        actualAbilityCooldown = 0f;
        abilityState = ABILITY.OFFCOOLDOWN;
        offCDSFXEvent.start();
        // play sound effect
    }

    private float refundFactor;
    public void GiveCooldownRefund()
    {
        if(abilityState != ABILITY.ONCOOLDOWN) return;
        refundFactor += refundForKill;
    }

    public virtual void ReceiveHitInfo(string tag)
    {
        if(tag == "Lethal") GiveCooldownRefund();
    }
}