using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GeneralSettings : MonoBehaviour
{
    [Header("FOV")]
    public Slider fovSlider;
    public TextMeshProUGUI fovNumber;

    [Header("Sensitivity")]
    public Slider sensSlider;
    public TextMeshProUGUI sensNumber;

    void Start()
    {
        fovSlider.value  = (float)PlayerPrefs.GetInt("FOV", 110);
        sensSlider.value = PlayerPrefs.GetFloat("Sensitivity", 5f);
    }

    public void FOVUpdated()
    {
        PlayerPrefs.SetInt("FOV", (int)fovSlider.value);
        fovNumber.text = fovSlider.value.ToString();
    }

    public void SensitivityUpdated()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensSlider.value);
        sensNumber.text = sensSlider.value.ToString("0.##");
    }
}
