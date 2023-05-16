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
    private float originalGravity;

    void Awake()
    {
        IsToggleable = true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(abilityKey))
        {
            if      (CanActivateAbility())   this.DoMovementAbility();
            else if (CanDeactivateAbility()) this.EndMovementAbility();
        }
    }

    protected override void DoMovementAbility()
    {
        base.DoMovementAbility();
        Time.timeScale = slowScale;                                           // set time scale
        chainShotCoroutine = StartChainShotWindow();                          // chain shot window
        StartCoroutine(chainShotCoroutine);
        originalGravity = Physics.gravity.y;                                  // gravity
        Physics.gravity = new Vector3(0f, Physics.gravity.y / 2, 0f);
        fovVFX.RevolverChainShotVFX();                                        // fov
        sfxEvent.start();                                                     // play SFX
        bgmController.LerpBGMPitch(0.1f, 1f, 0.1f);                           // change bgm pitch
        gunAttributes.gunShotSFXEvent.setPitch(slowScale);                    // change gun shot sfx
        offCDSFXEvent.setPitch(0.1f);                                         // change offcooldown sfx pitch
    }

    protected override void EndMovementAbility()
    {
        base.EndMovementAbility();
        if(chainShotCoroutine != null) StopCoroutine(chainShotCoroutine);     // stop chain shot
        Time.timeScale = 1f;                                                  // reset time scale
        Physics.gravity = new Vector3(0f, originalGravity, 0f);               // reset gravity to original
        fovVFX.UndoRevolverVFX();                                             // fov
        sfxEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);                    // stop sfx
        bgmController.LerpBGMPitch(1f, 0.1f, 0.1f);                           // change bgm pitch
        gunAttributes.gunShotSFXEvent.setPitch(1f);                           // change gun shot sfx pitch
        offCDSFXEvent.setPitch(1f);                                           // change offcooldown sfx pitch
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
