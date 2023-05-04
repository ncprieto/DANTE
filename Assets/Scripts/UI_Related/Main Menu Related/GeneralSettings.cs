using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GeneralSettings : MonoBehaviour
{
    public Slider fovSlider;
    public Slider sensSlider;

    void Start()
    {
        fovSlider.value  = (float)PlayerPrefs.GetInt("FOV", 110);
        sensSlider.value = PlayerPrefs.GetFloat("Sensitivity", 5f);
    }

    public void FOVUpdated()
    {
        PlayerPrefs.SetInt("FOV", (int)fovSlider.value);
    }

    public void SensitivityUpdated()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensSlider.value);
    }
}
