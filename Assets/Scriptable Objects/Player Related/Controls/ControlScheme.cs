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

    public void AddControlsToDictionary()
    {
        controls.Clear();
        for(int i = 0; i< names.Count; i++) controls.Add(names[i], values[i]);
        PrintControls();
    }

    public void SetControlsFrom(ControlScheme other)
    {
        names  = other.names;
        values = other.values;
        AddControlsToDictionary();
    }

    public void PrintControls()
    {
        foreach(var (key, value) in controls) Debug.Log(key + " " + (KeyCode)value);
    }

    public void AddKeyBind(string key, int ascii)
    {
        controls[key] = ascii;
        int index = name.FindIndex(a => a.Contains(key));
        Debug.Log("AT INDEX " + index);
        values[index] = ascii;
    }
}
