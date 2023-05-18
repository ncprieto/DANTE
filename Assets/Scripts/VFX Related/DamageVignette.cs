using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DamageVignette : MonoBehaviour
{
 
    UnityEngine.Rendering.Universal.Vignette vignette;
    UnityEngine.Rendering.VolumeProfile volumeProfile;

    private float prevIntensity;
    private float prevSmoothness;
    private Color prevColor;

    public bool isLowHP;

    // Start is called before the first frame update
    void Start()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if(!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        if(!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        isLowHP = false;
    }

    void Update()
    {
        if (isLowHP){
            vignette.intensity.Override(Mathf.Lerp(0.75f, 0f, Mathf.PingPong(Time.time, 0.5f)));
            vignette.smoothness.Override(1f);
            vignette.color.Override(Color.red);
        }
    }

    public IEnumerator DamageVFX()
    {
        if (vignette.color.value == Color.black || vignette.color.value == Color.yellow){
            prevIntensity = vignette.intensity.value;
            prevSmoothness = vignette.smoothness.value;
            prevColor = vignette.color.value;
        }
        vignette.intensity.Override(0.5f);
        vignette.smoothness.Override(1f);
        vignette.color.Override(Color.red);
        yield return new WaitForSeconds(.1f);
        vignette.intensity.Override(prevIntensity);
        vignette.smoothness.Override(prevSmoothness);
        vignette.color.Override(prevColor);
    }

    public IEnumerator HealVFX()
    {
        if (vignette.color.value == Color.black || vignette.color.value == Color.yellow){
            prevIntensity = vignette.intensity.value;
            prevSmoothness = vignette.smoothness.value;
            prevColor = vignette.color.value;
        }
        vignette.intensity.Override(.35f);
        vignette.smoothness.Override(1f);
        vignette.color.Override(Color.green);
        StartCoroutine(LerpIntensityDown(.35f, 0f, .1f));
        yield return new WaitForSeconds(.25f);
        vignette.intensity.Override(prevIntensity);
        vignette.smoothness.Override(prevSmoothness);
        vignette.color.Override(prevColor);
    }

    IEnumerator LerpIntensityDown(float start, float end, float t)
    {
        float elapsed = 0f;
        while (elapsed < t){
            if (vignette.color.value != Color.yellow){
                vignette.intensity.Override(Mathf.Lerp(start, end, elapsed * 4));
            }
            else{
                prevIntensity = vignette.intensity.value;
                prevSmoothness = vignette.smoothness.value;
                prevColor = vignette.color.value;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
