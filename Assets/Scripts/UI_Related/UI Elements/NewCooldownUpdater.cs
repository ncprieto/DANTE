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

    public void SetSliderAndNumber(float n)
    {
        slider.value = n;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        Debug.Log("here");
    }

    public void SetCooldownToReady()
    {
        timer.text = "";
        slider.value = 1f;
    }

    public void UpdateCooldown(float timeLeft, float baseTime)
    {
        slider.value = 1f - (timeLeft / baseTime);
        timer.text = Mathf.CeilToInt(timeLeft).ToString();
    }
}