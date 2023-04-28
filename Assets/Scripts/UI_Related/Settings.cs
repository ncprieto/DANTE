using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public ControlScheme defaultSettings;
    // Start is called before the first frame update
    void Start()
    {
       Debug.Log("HERE");
       if(PlayerPrefs.GetInt("FirstLoad") == 0)
       {
            PlayerPrefs.SetInt("FirstLoad", 1);
            // set all player prefs to the default values
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
}
