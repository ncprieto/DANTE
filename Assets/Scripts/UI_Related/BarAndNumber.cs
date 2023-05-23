using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarAndNumber : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI  number;
    
    public void SetSliderAndNumber(int n)
    {
        slider.value = n;
        fill.color   = gradient.Evaluate(slider.normalizedValue);
        number.text  = n.ToString();
    }
}
