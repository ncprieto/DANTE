using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHandler : MonoBehaviour
{
    public KeyCode escape;

    [Header ("Dependencies")]
    public LevelHandler lvlHandler;
    public PlayerHealth playerHealth;
    public UI_Script ui;

    [Header ("FadeIn/Out Variables")]
    public float fadeToOverlayTime;

    void Update()
    {
        if (Input.GetKeyDown(escape)) SceneManager.LoadScene(0);                              // transition to main scene when player exits
        if (lvlHandler.enemiesKilled >= lvlHandler.enemiesToKill) SceneManager.LoadScene(0);  // transition to temp scene
        if (playerHealth.playerCurrentHealth <= 0 || ui.timeLeft == -1) SceneManager.LoadScene(0); // go to temp transiton scene
    }

    IEnumerator FadeOverlay(float time, float start, float end)
    {
        float timeLeft = time;
        while(timeLeft > 0)
        {
            // fade alpha/background/overlay here
            timeLeft -= Time.deltaTime;
            yield return null;
        }
    }
}
