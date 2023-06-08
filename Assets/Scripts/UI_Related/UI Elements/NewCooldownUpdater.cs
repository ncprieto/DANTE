using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewCooldownUpdater : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public GameObject icon;
    public GameObject transIcon;
    public TextMeshProUGUI timer;
    public Image rechargedImage;
    public Image notReadyImage;

    private Color startColor;

    public void SetSliderAndNumber(float n)
    {
        slider.value = n;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetCooldownToReady()
    {
        timer.text = "";
        slider.value = 1f;
        StartCoroutine(FadeImageToZeroFrom(.75f, rechargedImage, .5f));
    }

    public void UpdateCooldown(float timeLeft, float baseTime)
    {
        slider.value = 1f - (timeLeft / baseTime);
        fill.color = gradient.Evaluate(slider.value);
        timer.text = Mathf.CeilToInt(timeLeft).ToString();
    }

    public void CooldownNotReadyYet()
    {
        StartCoroutine(FadeImageToZeroFrom(.9f, notReadyImage, .3f));
    }
    
    public void OnEnable(){
        startColor = new Color(rechargedImage.color.r, rechargedImage.color.g, rechargedImage.color.b, 0f);
        rechargedImage.color = startColor;
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
}