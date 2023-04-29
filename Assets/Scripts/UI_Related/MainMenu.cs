using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Text Fade Variable")]
    public float waitForTilFade;
    public float fadeDuration;
    public float refadeDuration;

    public List<GameObject> title;
    public List<GameObject> other;

    private IEnumerator fadeCoroutine;
    private bool buttonsEnabled;

    void Start()
    {
        fadeCoroutine = FadeTextAlpha(waitForTilFade, fadeDuration, title, 1f, 0f);
        StartCoroutine(fadeCoroutine);
        fadeCoroutine = FadeTextAlpha(waitForTilFade * 1.75f, fadeDuration, other, 1f, 0f);
        StartCoroutine(fadeCoroutine);
        UnlockCursor();
    }

    void OnAwake()
    {
        UnlockCursor();
    }

    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.A)) ToggleAllButtons();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
       Application.Quit();
    }

    public void SendToMainMenu()
    {

        SceneManager.LoadScene(0);
    }

    public void SendToVictory()
    {
        SceneManager.LoadScene(2);
    }

    public void SendToLose()
    {
        SceneManager.LoadScene(3);
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // public void ToggleAllButtons()
    // {
    //     foreach(GameObject button in other)                                   // disable button click and image on buttons
    //     {
    //         Button component = button.GetComponent<Button>();
    //         component.enabled = !component.enabled;
    //         Image image = button.GetComponent<Image>();
    //         image.enabled = !image.enabled;
    //     }

    //     if(fadeCoroutine != null) StopCoroutine(fadeCoroutine);
    //     float start = buttonsEnabled ? 1f : 0f;
    //     float end   = buttonsEnabled ? 0f : 1f;
    //     fadeCoroutine = FadeTextAlpha(0f, refadeDuration, other, start, end);  // fade button text down or up depending on if they are enabled
    //     StartCoroutine(fadeCoroutine);
    // }

    public IEnumerator FadeTextAlpha(float timeToWait, float duration, List<GameObject> elements, float start, float end)
    {
        yield return new WaitForSeconds(timeToWait);                               // wait and then start fading the alpha in
        float timeToFade = duration;
        while(timeToFade > 0f)
        {
            foreach(GameObject parent in elements)
            {
                TextMeshProUGUI text = parent.GetComponentInChildren<TextMeshProUGUI>();
                Color newAlpha = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(start, end, timeToFade / duration)); // lerp alpha based on duration of fade
                text.color = newAlpha;
            }
            timeToFade -= Time.deltaTime;
            yield return null;
        }
        buttonsEnabled = !buttonsEnabled;
    }
}
