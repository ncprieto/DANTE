using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVVFX : MonoBehaviour
{
    private float originalFOV;
    private float currentFOV;

    [Header ("Grapple FOV Variables")]
    public float grappleFOVTime;
    public float grappleFOVOffset;

    [Header ("Revolver FOV Variables")]
    public float revolverFOVTime;
    public float revolverFOVOffset;

    [Header ("B Hop FOV Variables")]
    public float bHopFOVTime;
    public float bHopFOVOffset;

    void Start()
    {
        originalFOV = Camera.main.fieldOfView;
    }

    void Update()
    {
        currentFOV = Camera.main.fieldOfView;
    }

    // Grapple Realted FOV Functions
    float FOVAtGrapple;
    IEnumerator grappleFOVCoroutine;
    public void GrappleStartVFX()
    {
        FOVAtGrapple = currentFOV;
        StartFOVVFX(grappleFOVCoroutine, currentFOV + grappleFOVOffset, currentFOV, grappleFOVTime);
    }

    public void GrappleEndVFX()
    {
        float start = currentFOV < FOVAtGrapple ? originalFOV : FOVAtGrapple;
        StartFOVVFX(grappleFOVCoroutine, start, currentFOV, grappleFOVTime);
    }

    // Revolver Related FOV Functions
    float FOVAtFirstShot;
    IEnumerator revolverFOVCoroutine;
    public void RevolverChainShotVFX(int shot)
    {
        if(shot == 2) FOVAtFirstShot = currentFOV;
        StartFOVVFX(revolverFOVCoroutine, currentFOV + revolverFOVOffset, currentFOV, revolverFOVTime);
    }

    public void UndoRevolverVFX()
    {
        float start = currentFOV < FOVAtFirstShot ? originalFOV : FOVAtFirstShot;
        StartFOVVFX(revolverFOVCoroutine, start, currentFOV, revolverFOVTime);
    }

    // B Hop Related FOV Functions
    float FOVAtBHop;
    IEnumerator bHopFOVCoroutine;
    public void BHopVFX(int count)
    {
        if(count == 1) FOVAtBHop = currentFOV;
        StartFOVVFX(bHopFOVCoroutine, currentFOV + bHopFOVOffset, currentFOV, bHopFOVTime);
    }

    public void UndoBHopVFX()
    {
        float start = currentFOV < FOVAtBHop ? originalFOV : FOVAtBHop;
        StartFOVVFX(bHopFOVCoroutine, start, currentFOV, bHopFOVTime);
    }

    private void StartFOVVFX(IEnumerator coroutine, float start, float end, float time)
    {
        if(coroutine != null) StopCoroutine(coroutine);
        coroutine = LerpFOV(start, end, time);
        StartCoroutine(coroutine);
    }

    IEnumerator LerpFOV(float start, float end, float timeFrame)
    {
        float timeLeft = timeFrame;
        while(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            Camera.main.fieldOfView = Mathf.Lerp(start, end, Mathf.Clamp(timeLeft / timeFrame, 0f, 1f));
            yield return null;
        }
    }
}