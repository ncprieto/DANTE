using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public ControlScheme defaultSettings;
    public ControlScheme playerSettings;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("FirstLoad") == 0)
        {
            defaultSettings.InitializeControls();
            playerSettings.InitializeControls();
            playerSettings.SetControlsFrom(defaultSettings);
            PlayerPrefs.SetInt("FirstLoad", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) playerSettings.UpdateControls("Forward", 59);
        if(Input.GetKeyDown(KeyCode.H)) SetControlsToDefault();
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("FirstLoad", 0);
    }

    public void OpenSettingsMenu()
    {
        Debug.Log("OPENING SETTINGS MENU");
    }

    public void SetControlsToDefault()
    {
        playerSettings.SetControlsFrom(defaultSettings);
    }
}
