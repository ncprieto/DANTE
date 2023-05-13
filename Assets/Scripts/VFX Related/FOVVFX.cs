using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVVFX : MonoBehaviour
{
    public Camera overlay;
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
    private float mainCurrentFOV;
    private float overlayCurrentFOV;

    void Awake()
    {
        Camera.main.fieldOfView = PlayerPrefs.GetInt("FOV", 110);
        originalFOV = Camera.main.fieldOfView;
    }

    void Start()
    {
        Camera.main.fieldOfView = PlayerPrefs.GetInt("FOV", 110);
        originalFOV = Camera.main.fieldOfView;
    }

    void Update()
    {
        mainCurrentFOV = Camera.main.fieldOfView;
        overlayCurrentFOV = overlay.fieldOfView;
    }

    // Grapple Realted FOV Functions
    IEnumerator mainGrappleFOV;
    IEnumerator overlayGrappleFOV;
    public void GrappleStartVFX()
    {
        if(IsCoroutineRunning("revolver")) return;
        mainGrappleFOV = DoFOVVFX(mainGrappleFOV, mainCurrentFOV + grappleFOVOffset, mainCurrentFOV, grappleFOVTime, "grapple-main", Camera.main);
        overlayGrappleFOV = DoFOVVFX(overlayGrappleFOV, 125f, overlayCurrentFOV, grappleFOVTime, "grapple-overlay", overlay);
    }

    public void GrappleEndVFX()
    {
        if(IsCoroutineRunning("revolver")) return;
        mainGrappleFOV = DoFOVVFX(mainGrappleFOV, originalFOV, mainCurrentFOV, grappleFOVTime, "grapple-main", Camera.main);
        overlayGrappleFOV = DoFOVVFX(overlayGrappleFOV, 100f, overlayCurrentFOV, grappleFOVTime, "grapple-overlay", overlay);
    }

    // Revolver Related FOV Functions
    IEnumerator mainRevolverFOV;
    IEnumerator overlayRevolverFOV;
    public void RevolverChainShotVFX()
    {
        if(IsCoroutineRunning("grapple")) StopCoroutine(mainGrappleFOV);
        mainRevolverFOV = DoFOVVFX(mainRevolverFOV, mainCurrentFOV - revolverFOVOffset, mainCurrentFOV, revolverStartUpTime, "revolver-main", Camera.main);
        overlayRevolverFOV = DoFOVVFX(overlayRevolverFOV, 79.2f, overlayCurrentFOV, revolverStartUpTime, "revolver-overlay", overlay);
    }

    public void UndoRevolverVFX()
    {
        mainRevolverFOV = DoFOVVFX(mainRevolverFOV, originalFOV, mainCurrentFOV, revolverEndTime, "revolver-main", Camera.main);
        overlayRevolverFOV = DoFOVVFX(overlayRevolverFOV, 100f, overlayCurrentFOV, revolverEndTime, "revolver-overlay", overlay);
    }


    /* DoFOVVFX() essesntially start and stops the FOV lerp. It stops a coroutine
     * when it detects that it is already running or will start a coroutine if it
     * is not running. Return a coroutine which is used to actually start and stop itself.
     */
    private IEnumerator DoFOVVFX(IEnumerator lerpCoroutine, float start, float end, float timeFrame, string name, Camera cam)
    {
        if(IsCoroutineRunning(name))
        {
            StopCoroutine(lerpCoroutine);
            SetCoroutine(name, false);
        }
        lerpCoroutine = LerpFOV(start, end, timeFrame, name, cam);
        StartCoroutine(lerpCoroutine);
        return lerpCoroutine;
    }

    IEnumerator LerpFOV(float start, float end, float timeFrame, string name, Camera cam)
    {
        SetCoroutine(name, true);
        float timeLeft = timeFrame;
        while(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            cam.fieldOfView = Mathf.Lerp(start, end, Mathf.Clamp(timeLeft / timeFrame, 0f, 1f));
            yield return null;
        }
        SetCoroutine(name, false);
    }

    // Some Boolean tracking functions to tell wheter a coroutine is running
    bool mainGrappleFOVLerping;
    bool overlayGrappleFOVLerping;
    bool mainRevolverFOVLerping;
    bool overlayRevolverFOVLerping;
    private void SetCoroutine(string name, bool state)
    {
        switch (name)
        {
            case "grapple-main":
                mainGrappleFOVLerping = state;
                break;
            case "grapple-overlay":
                overlayGrappleFOVLerping = state;
                break;
            case "revolver-main":
                mainRevolverFOVLerping = state;
                break;
            case "revolver-overlay":
                overlayRevolverFOVLerping = state;
                break;
        }
    }

    private bool IsCoroutineRunning(string name)
    {
        switch (name)
        {
            case "grapple-main":
                return mainGrappleFOVLerping;
            case "grapple-overlay":
                return overlayGrappleFOVLerping;
            case "revolver-main":
                return mainRevolverFOVLerping;
            case "revolver-overlay":
                return overlayRevolverFOVLerping;
        }
        return false;
    }
}