using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelector : MonoBehaviour
{
    public DifficultySettings easy;
    public DifficultySettings normal;
    public DifficultySettings hard;

    public void SetSettingsToEasy()
    {
        easy.SaveAllModifiers();
        StartGame();
    }

    public void SetSettingsToNormal()
    {
        normal.SaveAllModifiers();
        StartGame();
    }

    public void SetSettingsToHard()
    {
        hard.SaveAllModifiers();
        StartGame();
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
