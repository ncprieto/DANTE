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

    void Start()
    {
        FadeElements(title, fadeDuration, waitForTilFade);
        FadeElements(other, fadeDuration, waitForTilFade * 1.75f);
        UnlockCursor();
    }

    void OnAwake()
    {
        UnlockCursor();
    }

    public void QuitGame()
    {
       Application.Quit();
    }

    public void SendToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ToggleMenuElements()
    {
        FadeElements(title, refadeDuration, 0f);
        FadeElements(other, refadeDuration, 0f);
    }

    /* FadeElements() will fade text elements in and out based on if
     * they are/aren't active in the hierarchy. Accepts a list of elements
     * to fade, the duration of the fade, and a time to wait before starting the fade.
     */
    private void FadeElements(List<GameObject> elements, float fadeTime, float waitTime)
    {
        var (start, end) = GetStartAndEndValues(elements);
        IEnumerator fadeElements = FadeTextAlpha(fadeTime, elements, start, end);
        StartCoroutine(WaitForThenExecute(waitTime, fadeElements));
    }

    /* GetStartAndEndValues() returns a tuple that contains the start and
     * end values used for a linear interpolation. It's primarily used
     * for fading text elements in and out.
     */
    private (float, float) GetStartAndEndValues(List<GameObject> objs)
    {
        bool objsActive = AreObjectsActive(objs);
        float start = objsActive ? 0f : 1f;
        float end   = objsActive ? 1f : 0f;
        return (start, end);
    }

    /* AreObjectsActive() will return a boolean that determins if the GameObjects
     * in the provided list are active/inactive in the hierarchy.
     */
    private bool AreObjectsActive(List<GameObject> toCheck)
    {
        foreach(GameObject obj in toCheck) if(obj.activeSelf == false) return false;
        return true;
    }

    // WaitForThenExectute() will wait a duration and then execute a coroutine.
    private IEnumerator WaitForThenExecute(float seconds, IEnumerator coroutine)
    {
        yield return new WaitForSeconds(seconds);
        StartCoroutine(coroutine);
    }

    /* FadeTextAlpha() will fade the alpha of a text object in or out depending on
     * the values of the provided floats start and end. It will also disable the
     * GameObjects provided based on the start and end values.
     */
    private IEnumerator FadeTextAlpha(float duration, List<GameObject> elements, float start, float end)
    {
        if(start > end) foreach(GameObject parent in elements) parent.SetActive(!parent.activeSelf);
        if(start < end) ToggleButtonComponents(elements);

        float timeToFade = duration;
        bool buttonsToggled = false;
        while(timeToFade > 0f)
        {
            foreach(GameObject parent in elements)
            {
                TextMeshProUGUI text = parent.GetComponentInChildren<TextMeshProUGUI>();
                float t = Mathf.Lerp(start, end, timeToFade / duration);
                Color newAlpha = new Color(text.color.r, text.color.g, text.color.b, t); // lerp alpha based on duration of fade
                text.color = newAlpha;
                if(t > 0.25 && start > end && !buttonsToggled)
                {
                    buttonsToggled = true;
                    ToggleButtonComponents(elements);
                }
            }
            timeToFade -= Time.deltaTime;
            yield return null;
        }

        if(start < end) foreach(GameObject parent in elements) parent.SetActive(!parent.activeSelf);
    }


    private void ToggleButtonComponents(List<GameObject> Objects)
    {
        foreach(GameObject parent in Objects) 
        {
            Button potentialButton = parent.GetComponent<Button>();
            if(potentialButton != null) potentialButton.enabled = !potentialButton.enabled;
        }
    }
}
