using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider healthbarSlider;
    

    public void setSliderHealth(int n)
    {
        healthbarSlider.value = n;
    }
}
