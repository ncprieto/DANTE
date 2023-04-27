using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public float waitForTilFade;
    public float fadeDuration;

    public List<TextMeshProUGUI> titleGUIs;
    public List<TextMeshProUGUI> otherGUIs;

    void Start()
    {
        StartCoroutine(FadeTextToFullAlpha(waitForTilFade, fadeDuration, titleGUIs));
        StartCoroutine(FadeTextToFullAlpha(waitForTilFade * 1.75f, fadeDuration, otherGUIs));
    }

    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
       Application.Quit();
    }

    public IEnumerator FadeTextToFullAlpha(float timeToWait, float duration, List<TextMeshProUGUI> elements)
    {
        yield return new WaitForSeconds(timeToWait);                               // wait and then start fading the alpha in
        float timeToFade = duration;
        while(timeToFade > 0f)
        {
            foreach(TextMeshProUGUI text in elements)
            {
                Color newAlpha = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1f, 0f, timeToFade / duration)); // lerp alpha based on duration of fade
                text.color = newAlpha;
            }
            timeToFade -= Time.deltaTime;
            yield return null;
        }
    }
}
