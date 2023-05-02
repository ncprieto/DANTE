using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Control Scheme", menuName = "Player Related/Control Scheme", order = 0)]
public class ControlScheme : ScriptableObject
{
    [Header ("Key Value Pairs")]
    public List<string> names;
    public List<int>    values;
    private Dictionary<string, int> controls = new Dictionary<string, int>();

    public void InitializeControls()
    {
        for(int i = 0; i < names.Count; i++) controls.Add(names[i], values[i]);
    }

    public void WriteToPlayerPrefs()
    {
        Debug.Log("WRITING TO PLAYER PREFS");
        foreach(var (key, value) in controls) PlayerPrefs.SetInt(key, value);
    }

    public void SetControlsFrom(ControlScheme other)
    {
        Debug.Log("SETTING CONTROLS FROM OTHER SCHEME");
        this.controls = other.controls;
        foreach(var (key, value) in other.controls)
        {
            int index = names.FindIndex(a => a.Contains(key));
            Debug.Log(index);
            values[index] = value;
        }
        WriteToPlayerPrefs();
    }

    public void PrintPlayerPrefs()
    {
        foreach(var (key, value) in controls)
        {
            KeyCode num = (KeyCode)PlayerPrefs.GetInt(key);
            Debug.Log(key + " " + num);
        }
    }

    public void UpdateControls(string key, int asciiValue)
    {
        controls[key] = asciiValue;
        PlayerPrefs.SetInt(key, asciiValue);
        int index = names.FindIndex(a => a.Contains(key));
        values[index] = asciiValue;
        PrintPlayerPrefs();
    }
}
