using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVVFX : MonoBehaviour
{
    public Camera cam;
    private float originalFOV;

    void Start()
    {
        originalFOV = cam.fieldOfView;
    }

    private bool FOVLerpRunning;
    IEnumerator FOVLerpCoroutine;
    public void DoFOVLerpUp(float offset, float timeFrame)
    {
        float start = cam.fieldOfView;
        if(FOVLerpRunning) StopCoroutine(FOVLerpCoroutine);
        FOVLerpCoroutine = LerpFOV(start + offset, start, timeFrame);
        StartCoroutine(FOVLerpCoroutine);
    }

    public void DoFOVLerpDown(float offset, float timeFrame)
    {
        float end = cam.fieldOfView;
        if(FOVLerpRunning) StopCoroutine(FOVLerpCoroutine);
        FOVLerpCoroutine = LerpFOV(end - offset, end, timeFrame);
        StartCoroutine(FOVLerpCoroutine);
    }

    public void DoFOVLerpToOriginal(float timeFrame)
    {
        float end = cam.fieldOfView;
        if(FOVLerpRunning) StopCoroutine(FOVLerpCoroutine);
        FOVLerpCoroutine = LerpFOV(originalFOV, end, timeFrame);
        StartCoroutine(FOVLerpCoroutine);
    }

    IEnumerator LerpFOV(float start, float end, float timeFrame)
    {
        FOVLerpRunning = true;
        float timeLeft = timeFrame;
        while(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            cam.fieldOfView = Mathf.Lerp(start, end, timeLeft / timeFrame);
            yield return null;
        }
        FOVLerpRunning = false;
    }
}
