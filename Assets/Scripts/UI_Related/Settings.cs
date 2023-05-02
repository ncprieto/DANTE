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
        Debug.Log("START");
        Debug.Log(PlayerPrefs.GetInt("FirstLoad"));
        Debug.Log("SETTING UP DEFAULT CONTROLS");
        defaultSettings.AddControlsToDictionary();
        Debug.Log("SETTING UP PLAYER CONTROLS");
        playerSettings.AddControlsToDictionary();
        // playerSettings.SetControlsFrom(defaultSettings);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("FirstLoad", 0);
        Debug.Log("DESTORY");
        Debug.Log(PlayerPrefs.GetInt("FirstLoad"));
    }

    public void OpenSettingsMenu()
    {
        Debug.Log("OPENING SETTINGS MENU");
    }
}
