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
        if(grappleFOVCoroutine != null) StopCoroutine(grappleFOVCoroutine);
        grappleFOVCoroutine = LerpFOV(currentFOV + grappleFOVOffset, currentFOV, grappleFOVTime);
        StartCoroutine(grappleFOVCoroutine);
    }

    public void GrappleEndVFX()
    {
        if(grappleFOVCoroutine != null) StopCoroutine(grappleFOVCoroutine);
        float start = currentFOV < FOVAtGrapple ? originalFOV : FOVAtGrapple;    
        grappleFOVCoroutine = LerpFOV(start, currentFOV, grappleFOVTime);
        StartCoroutine(grappleFOVCoroutine);
    }

    // Revolver Related FOV Functions
    float FOVAtFirstShot;
    IEnumerator revolverFOVCoroutine;
    public void RevolverChainShotVFX(int shot)
    {
        if(shot == 2) FOVAtFirstShot = currentFOV;
        if(revolverFOVCoroutine != null) StopCoroutine(revolverFOVCoroutine);
        revolverFOVCoroutine = LerpFOV(currentFOV + revolverFOVOffset, currentFOV, revolverFOVTime);
        StartCoroutine(revolverFOVCoroutine);
    }

    public void UndoRevolverVFX()
    {
        if(revolverFOVCoroutine != null) StopCoroutine(revolverFOVCoroutine);
        float start = currentFOV < FOVAtFirstShot ? originalFOV : FOVAtFirstShot;
        revolverFOVCoroutine = LerpFOV(start, currentFOV, revolverFOVTime);
        StartCoroutine(revolverFOVCoroutine);
    }

    // B Hop Related FOV Functions
    float FOVAtBHop;
    IEnumerator bHopFOVCoroutine;
    public void BHopVFX(int count)
    {
        if(count == 1) FOVAtBHop = currentFOV;
        if(bHopFOVCoroutine != null) StopCoroutine(bHopFOVCoroutine);
        bHopFOVCoroutine = LerpFOV(currentFOV + bHopFOVOffset, currentFOV, bHopFOVTime);
        StartCoroutine(bHopFOVCoroutine);
    }

    public void UndoBHopVFX()
    {
        if(bHopFOVCoroutine != null) StopCoroutine(bHopFOVCoroutine);
        float start = currentFOV < FOVAtBHop ? originalFOV : FOVAtBHop;
        bHopFOVCoroutine = LerpFOV(start, currentFOV, bHopFOVTime);
        StartCoroutine(bHopFOVCoroutine);
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