using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Settings : MonoBehaviour
{
    public  ControlScheme controls;
    [Header("All Control Button/Text Object")]
    public  List<GameObject> controlsButtons;

    private bool listenForInput;
    private GameObject currentButton;

    void Awake()
    {
        controls.Awake();
        ConvertKeyCodesToReadable();
        LoadPrefsIntoText();                                                      // load playerprefs into the text objects
    }

    void Start()
    {
        listenForInput = false;
    }

    void Update()
    {
        if(listenForInput)                                                       // listen for additional mouse buttons because Event.current doesn't read these mouse 3-6
        {
            KeyCode newKey = KeyCode.None;
            if(Input.GetKeyDown(KeyCode.Mouse3))      newKey = KeyCode.Mouse3;
            else if(Input.GetKeyDown(KeyCode.Mouse4)) newKey = KeyCode.Mouse4;
            else if(Input.GetKeyDown(KeyCode.Mouse5)) newKey = KeyCode.Mouse5;
            else if(Input.GetKeyDown(KeyCode.Mouse6)) newKey = KeyCode.Mouse6;
            ReceiveInputCode(newKey);
        }
    }

    /* SetControlsToDefault() call controls.SetToDefault() to reset the controls.
     * It also stops listening for inputs.
     */
    public void SetControlsToDefault()
    {
        controls.SetToDefault();
        listenForInput = false;
        for(int i = 0; i < controlsButtons.Count; i++) UpdateButtonText(controlsButtons[i], (KeyCode)PlayerPrefs.GetInt(controlsButtons[i].name));
    }

    /* ReceiveButtonObject() is called from buttons in the scene. This
     * sets listenForInput to true so that certain if statements actively
     * listen for keyboard and mouse inputs from the user.
     */
    public void ReceiveButtonObject(GameObject button)
    {
        currentButton  = button;
        listenForInput = true;
    }

    /* ReceiveInputCode() will call controls.ChangeKeyBind() to change the
     * values stored in PlayerPrefs. It first must recognize that the passed
     * KeyCode isn't KeyCode.None to change the desired keybind under the hood.
     */
    private void ReceiveInputCode(KeyCode code)
    {
        if(code != KeyCode.None)
        {
            controls.ChangeKeyBind(currentButton.name, code);
            listenForInput = false;
            UpdateButtonText(currentButton, code);
        }
    }

    /* LoadPrefsIntoText() load the KeyCodes from PlayerPrefs into all
     * of the text elements in controlsButtons
     */
    private void LoadPrefsIntoText()
    {
        foreach(GameObject button in controlsButtons) UpdateButtonText(button, (KeyCode)PlayerPrefs.GetInt(button.name));
    }

    /* UpdateButtonText() changes the text of a the GameObject stored in 
     * currentButton. It accepts a KeyCode which it uses to access values
     * from the dictionary buttonNames.
     */
    private void UpdateButtonText(GameObject button, KeyCode code)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = buttonNames[code];
    }

    /* ConvertKeyCodesToReadable() converts the KeyCode Enum to more readable
     * version of strings which are stored in the dictionary buttonNames.
     */
    private Dictionary<KeyCode, string> buttonNames = new Dictionary <KeyCode, string>();
    private void ConvertKeyCodesToReadable()
    {
        foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
        {
            if(!buttonNames.ContainsKey(k)) buttonNames.Add(k, k.ToString());
        }
        // replace Alpha0, Alpha1, .. and Keypad0... with "0", "1", ...
        for (int i = 0; i < 10; i++){
            buttonNames[(KeyCode)((int)KeyCode.Alpha0+i)]  = i.ToString();
            buttonNames[(KeyCode)((int)KeyCode.Keypad0+i)] = i.ToString();
        }
        buttonNames[KeyCode.Comma] = ",";
        buttonNames[KeyCode.Escape] = "Esc";
    }

    void OnGUI()
    {
        if(listenForInput)
        {
            Event e = Event.current;
            KeyCode newKey = KeyCode.None;
            if(e.isKey)                          newKey = e.keyCode;
            else if(e.isMouse) if(e.button >= 0) newKey = (KeyCode)e.button + 323;         // add 323 for correct KeyCode enum
            ReceiveInputCode(newKey);
        }
    }
}
