using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class OOBDamage : MonoBehaviour
{
    public PlayerHealth playerHP;
    public bool isLimbo;

    [Header ("BGM")]
    public BGMController bgmController;

    [Header ("SFX Key and Event")]
    public string sfxKey;
    private FMOD.Studio.EventInstance sfxEvent;

    private float dotTimer;
    private float expCount;
    private bool inTrigger = true;

    void Awake()
    {
        sfxEvent = RuntimeManager.CreateInstance(sfxKey);
        dotTimer = 0;
        expCount = .01f;
    }

    void OnDestroy()
    {
        sfxEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    void Update(){
        if (!inTrigger && !isLimbo){
            dotTimer += Time.deltaTime;
            if (dotTimer > (.25f - expCount)){
                playerHP.ReceiveDamage(2, false);
                dotTimer = 0;
                expCount *= 1.035f;
                if (expCount > .25f){
                    expCount = .24f;
                }
            }   
        }
        else{
            dotTimer = 0;
            expCount = .01f;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player"  && !inTrigger)
        {
            inTrigger = true;
            bgmController.LerpBGMPitch(1f, 0.1f, 0.1f);                           // change bgm pitch
            bgmController.SetVolumeTo(1f);
            sfxEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" && inTrigger)
        {
            inTrigger = false;
            bgmController.LerpBGMPitch(0.1f, 1f, 0.1f);                           // change bgm pitch
            bgmController.SetVolumeTo(0.5f);
            sfxEvent.start();
        }
    }
}
