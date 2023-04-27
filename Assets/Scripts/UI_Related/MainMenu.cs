using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    float gameTime = 0;
    public double startTextFadeInTime;
    public float durationOfFade;


    float currentTime;
    float alpha;
    int timesDone;

    public GameObject startText;
    public TextMeshProUGUI startTextGUI;

    public GameObject quitText;
    public TextMeshProUGUI quitTextGUI;
    void Start()
    {
        startTextGUI = startText.GetComponent<TextMeshProUGUI>();
        quitTextGUI = quitText.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        //Debug.Log(startTextGUI);
        if (gameTime >= startTextFadeInTime && timesDone < 1)
        {
            //Debug.Log(startTextGUI);
            timesDone++;
            StartCoroutine(FadeTextToFullAlpha(durationOfFade, startTextGUI, quitTextGUI));

        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
       Application.Quit();
    }

    public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i, TextMeshProUGUI y)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        y.color = new Color(y.color.r, y.color.g, y.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            y.color = new Color(y.color.r, y.color.g, y.color.b, y.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
}
