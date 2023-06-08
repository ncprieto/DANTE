using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowTimePulse : MonoBehaviour
{
    public Image lowPulseImage;

    private bool isLowPulsing;
    private Vector3 origLPScale;
    private TimeUpdater timeUpdater;
    
    void Start(){
        isLowPulsing = false;
        origLPScale = lowPulseImage.gameObject.transform.localScale;
        timeUpdater = GameObject.Find("Canvas").GetComponent<TimeUpdater>();
    }

    void Update(){
        if (timeUpdater.timeLeft <= timeUpdater.warnPlayerOfTime){
            if (!isLowPulsing){
                StartCoroutine(LowPulse());
            }
        }
        else{
            isLowPulsing = false;
        }
    }

    private IEnumerator FadeImageToZeroFrom(float startAlpha, Image i, float t)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, startAlpha);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    private IEnumerator LowPulse(){
        isLowPulsing = true;
        lowPulseImage.gameObject.transform.localScale = origLPScale;
        StartCoroutine(FadeImageToZeroFrom(1f, lowPulseImage, 2f));
        while (lowPulseImage.gameObject.transform.localScale.y <= 2.5f){
            Vector3 newLPScale = new Vector3(lowPulseImage.gameObject.transform.localScale.x + (Time.deltaTime / 3f), lowPulseImage.gameObject.transform.localScale.y + (Time.deltaTime / 3f), lowPulseImage.gameObject.transform.localScale.z + (Time.deltaTime / 3f));
            lowPulseImage.gameObject.transform.localScale = newLPScale;
            yield return null;
        }
        isLowPulsing = false;
        yield return null;
    }
}
