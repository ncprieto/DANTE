using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AudioSettings : MonoBehaviour
{
    [Header("Master")]
    public Slider masterSlider;
    public TextMeshProUGUI masterNumber;

    [Header("Music")]
    public Slider musicSlider;
    public TextMeshProUGUI musicNumber;
    
    [Header("SFX")]
    public Slider sfxSlider;
    public TextMeshProUGUI sfxNumber;

    [Header("BGM")]
    public BGMController bgmController;

    void Start()
    {
        GetAndSetWithName("Master", masterSlider, masterNumber, 0.75f);
        GetAndSetWithName("Music",  musicSlider,  musicNumber,  1f);
        GetAndSetWithName("SFX",    sfxSlider,    sfxNumber,    1f);
    }

    public void MasterUpdated()
    {
        UpdateWithNameAndValue("Master", masterSlider.value, masterNumber);
        bgmController.UpdateOriginalVolume();
    }

    public void MusicUpdated()
    {
        UpdateWithNameAndValue("Music", musicSlider.value, musicNumber);
        bgmController.UpdateOriginalVolume();
    }

    public void SFXUpdated()
    {
        UpdateWithNameAndValue("SFX", sfxSlider.value, sfxNumber);
    }

    private void UpdateWithNameAndValue(string name, float value, TextMeshProUGUI number)
    {
        PlayerPrefs.SetFloat(name, value);
        number.text = (value * 100).ToString("0");
    }

    private void GetAndSetWithName(string name, Slider slider, TextMeshProUGUI number, float defaultValue)
    {
        float retrieved = PlayerPrefs.GetFloat(name, defaultValue);
        slider.value = retrieved;
        number.text  = (retrieved * 100).ToString("0");
    }
}
