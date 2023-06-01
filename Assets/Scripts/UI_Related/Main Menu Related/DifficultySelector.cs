using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelector : MonoBehaviour
{
    public List<GameObject> UIElements;

    public DifficultySettings easy;
    public DifficultySettings normal;
    public DifficultySettings hard;
    public DifficultySettings nightmare;

    public void SetSettingsToEasy()
    {
        SaveDifficulty(easy);
    }

    public void SetSettingsToNormal()
    {
        SaveDifficulty(normal);
    }

    public void SetSettingsToHard()
    {
        SaveDifficulty(hard);
    }

    public void SetSettingsToNightmare()
    {
        SaveDifficulty(nightmare);
    }

    private void SaveDifficulty(DifficultySettings difficulty)
    {
        difficulty.SaveAllModifiers();
    }
}
