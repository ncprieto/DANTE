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

    public void Awake()
    {
        for(int i = 0; i < keyNames.Count; i++) customCodes[i] = (KeyCode)PlayerPrefs.GetInt(keyNames[i], (int)defaultValues[i]);
        WriteAllToPlayerPrefs();
    }

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
        WriteAllToPlayerPrefs();
    }

    /* ChangeKeyBind() accepts a string and a KeyCode and updates the data in controls and
     * customCodes. It first checks if the string is found in keyNames and in controls.
     * If it isn't then the function exits early and now keybinds are changed.
     */
    public void ChangeKeyBind(string bindToChange, KeyCode newKeyCode)
    {
        int index = keyNames.FindIndex(bind => bind.Contains(bindToChange));
        if(index == -1) return;     
        customCodes[index] = newKeyCode;
        WriteSingleToPlayerPrefs(bindToChange, newKeyCode);
    }

    /* WriteAllToPlayerPrefs() takes all of the data found in controls and stores
     * it in PlayerPrefs. It converts all KeyCode values to ints so that they can
     * be stored properly.
     */
    private void WriteAllToPlayerPrefs()
    {
        for(int i = 0; i < keyNames.Count; i++) WriteSingleToPlayerPrefs(keyNames[i], customCodes[i]);
    }

    /* WriteSingleToPlayerPrefs() accepts a string and a KeyCode and sets a
     * single entry into PlayerPrefs.
     */
    private void WriteSingleToPlayerPrefs(string key, KeyCode code)
    {
        PlayerPrefs.SetInt(key, (int)code);
    }
}
