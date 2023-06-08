using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TimeRing : MonoBehaviour
{
    public float timeRingTimeAdded;
    public int waypointSystemIndex;
    public string collectSFX;
    public TimeSource source;
    private WaypointSystem ringWaypoint;
    private float sfxVolume;
    private FMOD.Studio.EventInstance collectSFXEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        ringWaypoint = Camera.main.GetComponents<WaypointSystem>()[waypointSystemIndex];
        ringWaypoint.target = this.transform;
        SetUpModifiers();
        SetUpAudio();
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Player"){
            source.ReceiveTimeFromSource(timeRingTimeAdded);
            ringWaypoint.target = null;
            collectSFXEvent.start();
            collectSFXEvent.release();
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }

    private void SetUpAudio()
    {
        sfxVolume = PlayerPrefs.GetFloat("Master", 0.75f) * PlayerPrefs.GetFloat("SFX", 1f);
        collectSFXEvent = RuntimeManager.CreateInstance(collectSFX);
        collectSFXEvent.setVolume(sfxVolume);
    }

    private void SetUpModifiers()
    {
        ApplyModifier("Time Per Ring", ref timeRingTimeAdded);
    }

    private void ApplyModifier(string modifierName, ref float value)
    {
        value *= PlayerPrefs.GetFloat(modifierName, 1);
    }

    private void ApplyModifier(string modifierName, ref int value)
    {
        value *= (int)PlayerPrefs.GetFloat(modifierName, 1);
    }
}
