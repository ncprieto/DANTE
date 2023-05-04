using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVVFX : MonoBehaviour
{
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

    private float originalFOV;
    private float currentFOV;

    void Awake()
    {
        Camera.main.fieldOfView = PlayerPrefs.GetInt("FOV", 90);
        originalFOV = Camera.main.fieldOfView;
    }

    void Start()
    {
        Camera.main.fieldOfView = PlayerPrefs.GetInt("FOV", 90);
        originalFOV = Camera.main.fieldOfView;
    }

    void Update()
    {
        currentFOV = Camera.main.fieldOfView;
    }

    // Grapple Realted FOV Functions
    IEnumerator grappleFOV;
    public void GrappleStartVFX()
    {
        if(IsCoroutineRunning("revolver")) return;
        grappleFOV = DoFOVVFX(grappleFOV, currentFOV + grappleFOVOffset, currentFOV, grappleFOVTime, "grapple");
    }

    public void GrappleEndVFX()
    {
        if(IsCoroutineRunning("revolver")) return;
        grappleFOV = DoFOVVFX(grappleFOV, originalFOV, currentFOV, grappleFOVTime, "grapple");
    }

    // Revolver Related FOV Functions
    IEnumerator revolverFOV;
    public void RevolverChainShotVFX()
    {
        if(IsCoroutineRunning("grapple")) StopCoroutine(grappleFOV);
        revolverFOV = DoFOVVFX(revolverFOV, currentFOV + revolverFOVOffset, currentFOV, revolverStartUpTime, "revolver");
    }

    public void UndoRevolverVFX()
    {
        revolverFOV = DoFOVVFX(revolverFOV, originalFOV, currentFOV, revolverEndTime, "revolver");
    }


    /* DoFOVVFX() essesntially start and stops the FOV lerp. It stops a coroutine
     * when it detects that it is already running or will start a coroutine if it
     * is not running. Return a coroutine which is used to actually start and stop itself.
     */
    private IEnumerator DoFOVVFX(IEnumerator lerpCoroutine, float start, float end, float timeFrame, string name)
    {
        if(IsCoroutineRunning(name))
        {
            StopCoroutine(lerpCoroutine);
            SetCoroutine(name, false);
        }
        lerpCoroutine = LerpFOV(start, end, timeFrame, name);
        StartCoroutine(lerpCoroutine);
        return lerpCoroutine;
    }

    IEnumerator LerpFOV(float start, float end, float timeFrame, string name)
    {
        SetCoroutine(name, true);
        float timeLeft = timeFrame;
        while(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            Camera.main.fieldOfView = Mathf.Lerp(start, end, Mathf.Clamp(timeLeft / timeFrame, 0f, 1f));
            yield return null;
        }
        SetCoroutine(name, false);
    }

    // Some Boolean tracking functions to tell wheter a coroutine is running
    bool grappleFOVLerping;
    bool revolverFOVLerping;
    private void SetCoroutine(string name, bool state)
    {
        switch (name)
        {
            case "grapple":
                grappleFOVLerping = state;
                break;
            case "revolver":
                revolverFOVLerping = state;
                break;
        }
    }

    private bool IsCoroutineRunning(string name)
    {
        switch (name)
        {
            case "grapple":
                return grappleFOVLerping;
            case "revolver":
                return revolverFOVLerping;
        }
        return false;
    }
}