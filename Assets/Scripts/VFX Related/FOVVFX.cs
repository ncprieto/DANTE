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

    bool grappleFOVLerping;
    float FOVAtGrapple;
    IEnumerator grappleFOVCoroutine;
    public void GrappleStartVFX()
    {
        FOVAtGrapple = currentFOV;
        grappleFOVCoroutine = LerpFOV(currentFOV + grappleFOVOffset, currentFOV, grappleFOVTime, grappleFOVLerping);
        StartCoroutine(grappleFOVCoroutine);
    }

    public void GrappleEndVFX()
    {
        if(grappleFOVLerping) StopCoroutine(grappleFOVCoroutine);
        float newOffset = Mathf.Min(currentFOV - FOVAtGrapple, grappleFOVOffset);
        grappleFOVCoroutine = LerpFOV(currentFOV - newOffset, currentFOV, grappleFOVTime, grappleFOVLerping);
        StartCoroutine(grappleFOVCoroutine);
    }

    bool revolverFOVLerping;
    float FOVAtRevolverShot;
    IEnumerator revolverFOVCoroutine;
    public void RevolverChainShotVFX()
    {
        Debug.Log("STARTING REVOLVER CHAIN SHOT VFX");
    }

    public void UndoRevolverVFX()
    {
        Debug.Log("UNDOING REVOLVER VFX");
    }

    IEnumerator LerpFOV(float start, float end, float timeFrame, bool tracker)
    {
        tracker = true;
        float timeLeft = timeFrame;
        while(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            Camera.main.fieldOfView = Mathf.Lerp(start, end, Mathf.Clamp(timeLeft / timeFrame, 0f, 1f));
            yield return null;
        }
        tracker = false;
    }
}
