using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeUpdater : MonoBehaviour
{
    [Header ("Time Variables")]
    public  GameObject UICanvas;
    public  GameObject TimerPrefab;
    public  GameObject AddTimerPrefab;
    public  GameObject BackgroundPrefab;
    public  float startingTime;
    public  float warnPlayerOfTime;
    private bool  unlimitedTime;
    private float timeLeft;

    // UI
    private GameObject Timer;
    private GameObject AddTimer;
    private GameObject Background;
    private TextMeshProUGUI TimerText;
    private TextMeshProUGUI AddTimerText;

    [Header ("Sources")]
    public TimeSource fromKills;
    public TimeSource fromTimeRings;

    void Start()
    {
        timeLeft   = startingTime;
        Background = Instantiate(BackgroundPrefab, UICanvas.transform, false);
        Timer      = Instantiate(TimerPrefab, UICanvas.transform, false);
        AddTimer   = Instantiate(AddTimerPrefab, UICanvas.transform, false);
        TimerText  = Timer.GetComponent<TextMeshProUGUI>();
        AddTimerText  = AddTimer.GetComponent<TextMeshProUGUI>();
        SetUpModifiers();
        InitializeSources();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9)) unlimitedTime = true;
        if (unlimitedTime) return;

        timeLeft -= Time.deltaTime;
        timeLeft = timeLeft > -1 ? timeLeft : -1f;
        updateTimerText(timeLeft);
        if (timeLeft <= warnPlayerOfTime) changeTextColor(TimerText, timeLeft);
    }

    public void ReceiveTime(float amount)
    {
        timeLeft += amount;
        AddTimerText.text = "+ " + amount.ToString() + " S";
        StartCoroutine(FadeTextToZeroAlpha(2f, AddTimerText));
    }

    public void updateTimerText(float currentTime)             // 80
    {
        currentTime += 1;

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int milliseconds = Mathf.FloorToInt((currentTime % 60 * 100) % 100);
        string secStr;
        string milliStr;
        secStr = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();
        milliStr = milliseconds < 10 ? ".0" + milliseconds.ToString() : "." + milliseconds.ToString();
        //update the text 
        TimerText.text = minutes.ToString() + ":" + secStr + milliStr;
    }

    public void changeTextColor(TextMeshProUGUI textToChange, float t)
    {   
        textToChange.color = (int)t % 2 == 0 ? Color.white : Color.red;
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    private void InitializeSources()
    {
        fromKills.Initialize(this);
        fromTimeRings.Initialize(this);
    }

    private void SetUpModifiers()
    {
        ApplyModifier("Starting Time", ref timeLeft);
    }

    private void ApplyModifier(string modifierName, ref float value)
    {
        value *= PlayerPrefs.GetFloat(modifierName, 1);
    }

    private void ApplyModifier(string modifierName, ref int value)
    {
        value *= (int)PlayerPrefs.GetFloat(modifierName, 1);
    }
}
