using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public  FMODUnity.StudioEventEmitter BGM;
    private FMOD.Studio.EventInstance    BGMEvent;
    private float originalVolume;
    
    void Start()
    {
        UpdateOriginalVolume();
        BGMEvent = BGM.EventInstance;
        BGMEvent.setVolume(originalVolume);
    }

    public void SetVolumeTo(float level)
    {
        BGMEvent.setVolume(level * originalVolume);
    }

    public void SetVolumeToOriginal()
    {
        BGMEvent.setVolume(originalVolume);
    }

    public void UpdateOriginalVolume()
    {
        originalVolume = PlayerPrefs.GetFloat("Master", 0.75f) * PlayerPrefs.GetFloat("Music", 1f);
        SetVolumeToOriginal();
    }

    public void LerpBGMPitch(float start, float end, float timeFrame)
    {
        if(BGMPitchLerp != null) StopCoroutine(BGMPitchLerp);
        BGMPitchLerp = LerpPitch(start, end, timeFrame);
        StartCoroutine(BGMPitchLerp);
    }

    // lerp pitch coroutine
    IEnumerator BGMPitchLerp;
    IEnumerator LerpPitch(float start, float end, float timeFrame)
    {
        float timeLeft = timeFrame;
        while(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            float pitchLevel = Mathf.Lerp(start, end, timeLeft / timeFrame);
            BGMEvent.setPitch(pitchLevel);
            yield return null;
        }
    }
}
