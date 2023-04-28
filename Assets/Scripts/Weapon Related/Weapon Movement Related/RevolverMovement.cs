using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverMovement : GunMovement
{
    [Header("Movement Effect Variables")]
    public float slowScale;
    public float baseSlowTime;
    public float timePerNormal;
    public float timePerCrit;

    private float timeToAdd;

    void Awake()
    {
        IsToggleable = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(abilityKey))
        {
            if(CanActivateAbility())        this.DoMovementAbility();
            else if(CanDeactivateAbility()) this.EndMovementAbility();
        } 
    }

    protected override void DoMovementAbility()
    {
        base.DoMovementAbility();
        Time.timeScale = slowScale;
        chainShotCoroutine = StartChainShotWindow();
        StartCoroutine(chainShotCoroutine);
        fovVFX.RevolverChainShotVFX();
    }

    protected override void EndMovementAbility()
    {
        base.EndMovementAbility();
        if(abilityState == ABILITY.ACTIVE) StopCoroutine(chainShotCoroutine);
        Time.timeScale = 1f;
        fovVFX.UndoRevolverVFX();
    }

    public override void ReceiveHitInfo(string tag)
    {
        if(tag == null && abilityState == ABILITY.ACTIVE)                        // if players miss and the ability is active, end the ability
        {
            if(CanDeactivateAbility())
            {
                this.EndMovementAbility();
                return;
            } 
        }
        base.ReceiveHitInfo(tag);
        if(tag == "NormalHitbox") timeToAdd = timePerNormal;
        if(tag == "CritHitbox" || tag == "Lethal")   timeToAdd = timePerCrit;
    }

    IEnumerator chainShotCoroutine;
    IEnumerator StartChainShotWindow()
    {
        float timeLeft = baseSlowTime;
        while(timeLeft > 0)
        {
            if(timeToAdd > 0)                      // add time to timer if player chains shots together
            {
                timeLeft += timeToAdd;
                timeToAdd = 0f;
            }
            else timeLeft -= Time.deltaTime;
            yield return null;
        }
        this.EndMovementAbility();
    }
}
