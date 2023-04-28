using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Control Scheme", menuName = "Player Related/Control Scheme", order = 0)]
public class ControlScheme : ScriptableObject
{
    [Header ("Default Control Scheme")]
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode grapple;
    public KeyCode abilityKey;

    public void SetControlsToDefault()
    {
        Debug.Log("SETTING CONTROLS TO DEFAULT");
    }
}
