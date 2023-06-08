using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class RevolverMovement : GunMovement
{
    [Header("Movement Effect Variables")]
    public float slowScale;
    public float baseSlowTime;
    public float timePerNormal;
    public float timePerCrit;

    private float timeToAdd;
    private float originalGravity;

    // VFX stuff
    private UnityEngine.Rendering.VolumeProfile globalVolumeProfile;
    private UnityEngine.Rendering.VolumeProfile localVolumeProfile;
    private UnityEngine.Rendering.Universal.Vignette globalVignette;
    private UnityEngine.Rendering.Universal.Vignette localVignette;
    private UnityEngine.Rendering.Universal.MotionBlur motionBlur;
    private UnityEngine.Rendering.Universal.Bloom bloom;

    private float gVigBaseIntensity;
    private Color gVigBaseColor;
    private float lVigBaseIntensity;
    private Color lVigBaseColor;
    private float baseBloomThreshold;

    [Header("Ability Cooldown Slider")]
    public GameObject AbilitySliderPrefab;
    private GameObject AbilitySlider;

    public override void Initialize(GameObject playerObj, GunAttributes ga, GameObject soundSystem, GameObject Canvas)
    {
        base.Initialize(playerObj, ga, soundSystem, Canvas);
        AbilitySlider = Instantiate(AbilitySliderPrefab, UICanvas.transform, false);
        AbilitySlider.SetActive(false);
        SetUpPostProcessing();
        IsToggleable = true;
        originalGravity = Physics.gravity.y;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Time.timeScale = 1f;
        Physics.gravity = new Vector3(0f, originalGravity, 0f);
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
        DoStartVFX();
        DoStartSFX();
        AbilitySlider.SetActive(true);
    }

    protected override void EndMovementAbility()
    {
        base.EndMovementAbility();
        if(chainShotCoroutine != null) StopCoroutine(chainShotCoroutine);     // stop chain shot
        Time.timeScale = 1f;                                                  // reset time scale
        Physics.gravity = new Vector3(0f, originalGravity, 0f);               // reset gravity to original
        DoEndVFX();
        DoEndSFX();
        AbilitySlider.SetActive(false);
    }

    public override void ReceiveHitInfo(string tag)
    {
        if(tag == null && abilityState == ABILITY.ACTIVE)                     // if players miss and the ability is active, end the ability
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
            AbilitySlider.transform.localScale = new Vector3(timeLeft / baseSlowTime, 1, 1);
            yield return null;
        }
        this.EndMovementAbility();
    }

    public override void EnableUI()
    {
        AbilitySlider.SetActive(true);
        base.EnableUI();
    }

    public override void DisableUI()
    {
        AbilitySlider.SetActive(false);
        base.DisableUI();
    }

    private void SetUpPostProcessing()
    {
        globalVolumeProfile = GameObject.Find("Global Volume").GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if(!globalVolumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        if(!globalVolumeProfile.TryGet(out globalVignette)) throw new System.NullReferenceException(nameof(globalVignette));

        if(!globalVolumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        if(!globalVolumeProfile.TryGet(out motionBlur)) throw new System.NullReferenceException(nameof(motionBlur));

        if(!globalVolumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        if(!globalVolumeProfile.TryGet(out bloom)) throw new System.NullReferenceException(nameof(bloom));

        localVolumeProfile = GameObject.Find("Local Volume").GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if(!localVolumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        if(!localVolumeProfile.TryGet(out localVignette)) throw new System.NullReferenceException(nameof(localVignette));

        gVigBaseIntensity = globalVignette.intensity.value;
        gVigBaseColor = globalVignette.color.value;
        lVigBaseIntensity = localVignette.intensity.value;
        lVigBaseColor = localVignette.color.value;
        baseBloomThreshold = bloom.threshold.value;
    }

    private void DoStartVFX()
    {
        fovVFX.RevolverChainShotVFX();                                        // fov
        motionBlur.intensity.Override(0.5f);                                  // post-processing stuff
        globalVignette.intensity.Override(0.75f);
        globalVignette.color.Override(Color.yellow);
        localVignette.intensity.Override(0.75f);
        localVignette.color.Override(Color.yellow);
        bloom.threshold.Override(0.45f);
    }

    private void DoEndVFX()
    {
        fovVFX.UndoRevolverVFX();                                             // fov
        motionBlur.intensity.Override(0f);                                    // post-processing stuff
        globalVignette.intensity.Override(gVigBaseIntensity);
        globalVignette.color.Override(gVigBaseColor);
        localVignette.intensity.Override(lVigBaseIntensity);
        localVignette.color.Override(lVigBaseColor);
        bloom.threshold.Override(baseBloomThreshold);
    }

    private void DoStartSFX()
    {
        sfxEvent.start();                                                     // play SFX
        startSFXEvent.start();
        bgmController.LerpBGMPitch(0.1f, 1f, 0.1f);                           // change bgm pitch
        gunAttributes.gunShotSFXEvent.setPitch(slowScale);                    // change gun shot sfx
        offCDSFXEvent.setPitch(0.1f);                                         // change offcooldown sfx pitch
    }

    private void DoEndSFX()
    {
        sfxEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);                    // stop sfx
        startSFXEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        bgmController.LerpBGMPitch(1f, 0.1f, 0.1f);                           // change bgm pitch
        gunAttributes.gunShotSFXEvent.setPitch(1f);                           // change gun shot sfx pitch
        offCDSFXEvent.setPitch(1f);                                           // change offcooldown sfx pitch
    }
}
