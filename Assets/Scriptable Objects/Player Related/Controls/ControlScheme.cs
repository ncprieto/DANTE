using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Control Scheme", menuName = "Player Related/Control Scheme", order = 0)]
public class ControlScheme : ScriptableObject
{
    [Header ("KeyBind Name and Value Pairs")]
    public List<string>  keyNames;
    public List<KeyCode> customCodes;
    public List<KeyCode> defaultCodes;                                                          // list for default key binds DO NOT CHANGE ONLY ADD TO IN EDITOR ONLY ACCESS VALUES FROM defaultValues
    public IReadOnlyList<KeyCode> defaultValues => defaultCodes as IReadOnlyList<KeyCode>;      // makes defaultCodes read only so default controls can't be edited accidentally at runtime
    private Dictionary<string, KeyCode> controls = new Dictionary<string, KeyCode>();

    /* SetToDefault() will set all values in customCodes to the values within
     * defaultCodes. It accesses defaultValues as a means of accessing the values
     * in defaultCodes.
     */
    public void SetToDefault()
    {
        if(customCodes.Count != defaultCodes.Count)                                             // if custom codes isn't same length as default clear it and add values to it
        {
            customCodes.Clear();
            for(int i = 0; i < defaultValues.Count; i++) customCodes.Add(defaultValues[i]);
        }
        else for(int i = 0; i < defaultValues.Count; i++) customCodes[i] = defaultValues[i];    // else set indices to be the same as default
        UpdateAllEntries();
    }

    /* UdateAllEntries() iterates through keyNames and sets its contents to be the
     * keys in the controls dictionary. It also sets each key to have the correctly
     * associated KeyCode value.
     */
    private void UpdateAllEntries()
    {
        for(int i = 0; i < keyNames.Count; i++) controls[keyNames[i]] = customCodes[i];
        WriteAllToPlayerPrefs();
    }

    /* ChangeKeyBind() accepts a string and a KeyCode and updates the data in controls and
     * customCodes. It first checks if the string is found in keyNames and in controls.
     * If it isn't then the function exits early and now keybinds are changed.
     */
    public void ChangeKeyBind(string bindToChange, KeyCode newKeyCode)
    {
        int index   = keyNames.FindIndex(bind => bind.Contains(bindToChange));
        bool inDict = controls.ContainsKey(bindToChange);
        if(index == -1 || !inDict) return;                                                       // if the keybind we want to change isn't in controls of keyNames then exit 
        controls[bindToChange] = newKeyCode;
        customCodes[index]     = newKeyCode;
        WriteSingleToPlayerPrefs(bindToChange, newKeyCode);
    }

    /* WriteAllToPlayerPrefs() takes all of the data found in controls and stores
     * it in PlayerPrefs. It converts all KeyCode values to ints so that they can
     * be stored properly.
     */
    private void WriteAllToPlayerPrefs()
    {
        Debug.Log("WRITING ALL TO PLAYER PREFS");
        foreach(var (key, value) in controls) PlayerPrefs.SetInt(key, (int)value);
        PrintAllFromPlayerPrefs();
    }

    /* WriteSingleToPlayerPrefs() accepts a string and a KeyCode and sets a
     * single entry into PlayerPrefs.
     */
    private void WriteSingleToPlayerPrefs(string key, KeyCode code)
    {
        Debug.Log("WRITING SINGLE TO PLAYER PREFS");
        PlayerPrefs.SetInt(key, (int)code);
        PrintSingleFromPlayerPrefs(key);
    }

    // Functions for printing out the data found in controls and PlayerPrefs.
    private void PrintDictionary()
    {
        Debug.Log("PRINTING DICTIONARY");
        foreach(var (key, value) in controls) Debug.Log("ACTION: " + key + " KEYCODE: " + value);
    }

    private void PrintAllFromPlayerPrefs()
    {
        Debug.Log("PRINTING ALL FROM PLAYER PREFS");
        foreach(var (key, value) in controls) Debug.Log("ACTION: " + key + " KEYCODE: " + (KeyCode)PlayerPrefs.GetInt(key, (int)value));
    }

    private void PrintSingleFromPlayerPrefs(string action)
    {
        Debug.Log("PRINTING SINGLE FROM PLAYER PREFS");
        Debug.Log("ACTION: " + action + " KEYCODE :" + (KeyCode)PlayerPrefs.GetInt(action));
    }

    // Functions for clearing data in PlayerPrefs
    private void ClearAllFromPlayerPrefs()
    {
        Debug.Log("CLEARING ALL FROM PLAYER PREFS");
        foreach(var (key, value) in controls) PlayerPrefs.SetInt(key, 0);
    }

    private void ClearSingleFromPlayerPrefs(string action)
    {
        PlayerPrefs.SetInt(action, 0);
    }
}
