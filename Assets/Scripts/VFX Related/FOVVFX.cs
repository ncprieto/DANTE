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

    void Start()
    {
        originalFOV = Camera.main.fieldOfView;
    }

    void Update()
    {
        currentFOV = Camera.main.fieldOfView;
    }

    float FOVAtGrapple;
    IEnumerator grappleFOVCoroutine;
    public void GrappleStartVFX()
    {
        if(grappleFOVCoroutine != null) StopCoroutine(grappleFOVCoroutine);
        FOVAtGrapple = currentFOV;
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

    float FOVAtFirstShot;
    IEnumerator revolverFOVCoroutine;
    public void RevolverChainShotVFX(int shot)
    {
        if(revolverFOVCoroutine != null) StopCoroutine(revolverFOVCoroutine);
        if(shot == 2) FOVAtFirstShot = currentFOV;
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