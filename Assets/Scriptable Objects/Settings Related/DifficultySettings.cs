using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Difficulty Setting", menuName = "Settings Related/Difficulty Setting", order = 0)]
public class DifficultySettings : ScriptableObject
{
    [Header("Enemy Related Modifiers")]
    public List<string> enemyModifierNames;
    public List<float>  enemyValues;

    [Header("Player Related Modifiers")]
    public List<string> playerModifierNames;
    public List<float>  playerValues;

    [Header("General Related Modifiers")]
    public List<string> generalModifierNames;
    public List<float>  generalValues;

    public void SaveAllModifiers()
    {
        WriteToPlayerPrefs(enemyModifierNames, enemyValues);
        WriteToPlayerPrefs(playerModifierNames, playerValues);
        WriteToPlayerPrefs(generalModifierNames, generalValues);
    }

    public void WriteToPlayerPrefs(List<string> names, List<float> values)
    {
        for(int i = 0; i < names.Count; i++) WriteModifierToPlayerPrefs(names[i], values[i]);
    }

    public void WriteModifierToPlayerPrefs(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
    }
}
