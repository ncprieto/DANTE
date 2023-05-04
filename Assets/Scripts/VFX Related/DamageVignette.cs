using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DamageVignette : MonoBehaviour
{
 
    UnityEngine.Rendering.Universal.Vignette vignette;
    UnityEngine.Rendering.VolumeProfile volumeProfile;

    // Start is called before the first frame update
    void Start()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if(!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        if(!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
    }

    public IEnumerator DamageVFX()
    {
        vignette.intensity.Override(0.5f);
        vignette.smoothness.Override(1f);
        vignette.color.Override(Color.red);
        yield return new WaitForSeconds(.1f);
        vignette.intensity.Override(0.2f);
        vignette.smoothness.Override(0.5f);
        vignette.color.Override(Color.black);
    }
}
