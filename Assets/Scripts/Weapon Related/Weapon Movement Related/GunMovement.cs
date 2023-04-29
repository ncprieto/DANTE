using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMovement : MonoBehaviour
{
    [Header ("Base Class Variables")]
    public KeyCode abilityKey;
    public float abilityCooldown;
    public float actualAbilityCooldown;
    public bool  IsToggleable;
    public float refundForKill;

    // External stuff
    protected GameObject player;
    protected Rigidbody rb;
    protected Camera playerCamera;
    protected Movement movement;
    protected GunAttributes gunAttributes;
    protected FOVVFX fovVFX;

    // Enum for keeping track on the current start of the ability
    protected ABILITY abilityState;
    protected enum ABILITY
    {
        ACTIVE,
        ONCOOLDOWN,
        OFFCOOLDOWN
    }
    
    public void Initialize(GameObject playerObj, GunAttributes ga)
    {
        player = playerObj;
        rb = player.GetComponent<Rigidbody>();
        movement = player.GetComponent<Movement>();
        fovVFX = player.GetComponent<FOVVFX>();
        gunAttributes = ga;
        abilityState = ABILITY.OFFCOOLDOWN;
    }

    protected virtual void DoMovementAbility()
    {
        abilityState = ABILITY.ACTIVE;
        if(!IsToggleable) StartCoroutine(StartAbilityCooldown());                             // if ability is one time use aka not toggleable start cooldown
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