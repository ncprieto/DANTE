using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    public GameObject DifficultyName;
    private TextMeshProUGUI DifficultyText;

    void OnEnable()
    {
        DifficultyText = DifficultyName.GetComponent<TextMeshProUGUI>();
        DifficultyText.text = PlayerPrefs.GetString("Difficulty Name");
    }

    public void OnLevelSelected(string name)
    {
        SceneManager.LoadScene(name + "Level", LoadSceneMode.Single);
    }
}
