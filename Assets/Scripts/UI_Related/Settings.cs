using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public ControlScheme controls;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("ControlsInit") == 0)
        {
            controls.SetToDefault();
            PlayerPrefs.SetInt("ControlsInit", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSettingsMenu()
    {
        Debug.Log("OPENING SETTINGS MENU");
    }

    public void SetControlsToDefault()
    {
        controls.SetToDefault();
    }
}
