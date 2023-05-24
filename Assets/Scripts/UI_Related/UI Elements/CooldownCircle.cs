using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CooldownCircle : MonoBehaviour
{
    public Slider slider;
    public Image  fill;
    public Color  fillColor;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI cdName;

    public void InitializeCooldown(string name)
    {
        SetCooldownToReady();
        cdName.text = name;
        fill.color  = fillColor;
    }

    public void SetCooldownToReady()
    {
        slider.value = 1f;
        timer.text   = "Ready";
    }

    public void UpdateCooldown(float timeLeft, float baseTime)
    {
        slider.value = 1f - (timeLeft / baseTime);
        timer.text   = Mathf.CeilToInt(timeLeft).ToString();
    }
}
