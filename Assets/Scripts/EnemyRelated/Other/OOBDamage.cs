using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class OOBDamage : MonoBehaviour
{
    public PlayerHealth playerHP;

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
        if (!inTrigger){
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
            sfxEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            bgmController.SetVolumeTo(1f);
            bgmController.LerpBGMPitch(1f, 0.1f, 0.1f);                           // change bgm pitch
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" && inTrigger)
        {
            inTrigger = false;
            sfxEvent.start();
            bgmController.SetVolumeTo(0.2f);
            bgmController.LerpBGMPitch(0.1f, 1f, 0.1f);                           // change bgm pitch
        }
    }
}
