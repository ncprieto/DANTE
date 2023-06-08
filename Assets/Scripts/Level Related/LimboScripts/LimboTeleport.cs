using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class LimboTeleport : MonoBehaviour
{
    public Transform tpTo;
    public GameObject prevSegment;
    public GameObject nextSegment;

    public LimboHandler limboHandler;
    public LimboOverlays limboOverlays;

    public string teleportSFX;
    private float sfxVolume;
    private FMOD.Studio.EventInstance teleportSFXEvent;

    private UnityEngine.Object tpParticles;

    // Start is called before the first frame update
    void Start()
    {
        tpParticles = Resources.Load("Prefabs/TeleportSmokeParticles");
        SetUpAudio();
    }

    void OnCollisionEnter(Collision col){
        if (col.gameObject.tag == "Player"){
            limboHandler.currentLimboObj++;
            limboHandler.objChanged = true;
            col.gameObject.transform.position = tpTo.position;
            Instantiate(tpParticles, col.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f), col.gameObject.transform);
            limboOverlays.runCompleteMat = true;
            nextSegment.SetActive(true);
            prevSegment.SetActive(false);
            teleportSFXEvent.start();
            teleportSFXEvent.release();
        }
    }

    private void SetUpAudio()
    {
        sfxVolume = PlayerPrefs.GetFloat("Master", 0.75f) * PlayerPrefs.GetFloat("SFX", 1f);
        teleportSFXEvent = RuntimeManager.CreateInstance(teleportSFX);
        teleportSFXEvent.setVolume(sfxVolume);
        
    }
}
