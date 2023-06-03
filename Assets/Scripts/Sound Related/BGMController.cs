using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public  FMODUnity.StudioEventEmitter BGM;
    private FMOD.Studio.EventInstance    BGMEvent;
    // Start is called before the first frame update
    void Start()
    {
        // get volume from player prefs
        BGMEvent = BGM.EventInstance;
        BGMEvent.setVolume(1f);
    }

    public void SetVolumeTo(float level)
    {
        BGMEvent.setVolume(level);
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
