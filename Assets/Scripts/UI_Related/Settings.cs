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
        Debug.Log(PlayerPrefs.GetInt("FirstLoad"));
        if(PlayerPrefs.GetInt("FirstLoad") == 0)
        {
            defaultSettings.InitializeControls();
            defaultSettings.WriteToPlayerPrefs();
            playerSettings.SetControlsFrom(defaultSettings);
            PlayerPrefs.SetInt("FirstLoad", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("FirstLoad", 0);
        Debug.Log(PlayerPrefs.GetInt("FirstLoad"));
    }

    public void OpenSettingsMenu()
    {
        Debug.Log("OPENING SETTINGS MENU");
    }

    private void SetSettings(ControlScheme controls)
    {

    }
}
