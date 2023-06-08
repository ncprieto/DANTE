using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelTextFade : MonoBehaviour
{

    public Image textBg;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitThenFadeImageToZeroFrom(.8f, textBg, 2f, 2f));
        StartCoroutine(WaitThenFadeTextToZeroAlpha(2f, text, 2f));
        StartCoroutine(WaitThenDestroyObject(4.5f));
    }

    public IEnumerator WaitThenFadeImageToZeroFrom(float startAlpha, Image i, float t, float wait)
    {
        yield return new WaitForSeconds(wait);
        i.color = new Color(i.color.r, i.color.g, i.color.b, startAlpha);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator WaitThenFadeTextToZeroAlpha(float t, TextMeshProUGUI i, float wait)
    {
        yield return new WaitForSeconds(wait);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator WaitThenDestroyObject(float wait)
    {
        yield return new WaitForSeconds(wait);
        Destroy(gameObject);
    }
}
