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
    public float revolverStartUpTime;
    public float revolverEndTime;
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
        if(revolverFOVCoroutine != null) return;
        if(grappleFOVCoroutine  != null) StopCoroutine(grappleFOVCoroutine);
        grappleFOVCoroutine = LerpFOV(currentFOV + grappleFOVOffset, currentFOV, grappleFOVTime);
        StartCoroutine(grappleFOVCoroutine);
    }

    public void GrappleEndVFX()
    {
        if(revolverFOVCoroutine != null) return;
        if(grappleFOVCoroutine != null) StopCoroutine(grappleFOVCoroutine);
        grappleFOVCoroutine = LerpFOV(originalFOV, currentFOV, grappleFOVTime);
        StartCoroutine(grappleFOVCoroutine);
    }

    // Revolver Related FOV Functions
    float FOVAtFirstShot;
    IEnumerator revolverFOVCoroutine;
    public void RevolverChainShotVFX()
    {
        if(grappleFOVCoroutine  != null) StopCoroutine(grappleFOVCoroutine);
        if(revolverFOVCoroutine != null) StopCoroutine(revolverFOVCoroutine);
        revolverFOVCoroutine = LerpFOV(currentFOV + revolverFOVOffset, currentFOV, revolverStartUpTime);
        StartCoroutine(revolverFOVCoroutine);
    }

    public void UndoRevolverVFX()
    {
        if(revolverFOVCoroutine != null) StopCoroutine(revolverFOVCoroutine);
        revolverFOVCoroutine = LerpFOV(originalFOV, currentFOV, revolverEndTime);
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