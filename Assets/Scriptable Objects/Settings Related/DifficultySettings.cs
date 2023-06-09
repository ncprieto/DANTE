using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Difficulty Setting", menuName = "Settings Related/Difficulty Setting", order = 0)]
public class DifficultySettings : ScriptableObject
{
    public string difficultyName;

    [Header("Enemy Related Modifiers")]
    public List<string> enemyModifierNames = new List<string>()
    {
        "Enemy Speed",
        "Spawn Rate Time",
        "Spawn Amount"
    };
    public List<float>  enemyValues = new List<float>()
    {
        1f,
        1f,
        1f
    };

    [Header("Player Related Modifiers")]
    public List<string> playerModifierNames = new List<string>()
    {
        "Incoming Damage"
    };
    public List<float>  playerValues = new List<float>()
    {
        1f
    };

    [Header("General Related Modifiers")]
    public List<string> generalModifierNames = new List<string>()
    {
        "Time Per Ring",
        "Starting Time",
        "Enemies To Kill",
        "Par Time"
    };
    public List<float>  generalValues = new List<float>()
    {
        1f,
        1f,
        1f,
        1f
    };

    public void SaveAllModifiers()
    {
        PlayerPrefs.SetString("Difficulty Name", difficultyName);
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
