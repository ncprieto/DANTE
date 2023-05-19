using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Script : MonoBehaviour
{
    public Rigidbody player;
    public GameObject speedText;

    //timer stuff
    [Header("Timer Related")]
    public GameObject timerText;
    public bool timerOn;
    public float timeLeft;
    public int warnPlayerOfTime;
    public bool unlimitedTime;

    //in game speed component
    private TextMeshProUGUI gameSpeedText;
    private TextMeshProUGUI gameTimerText;
    private TextMeshProUGUI timeAdditionText;
    // Start is called before the first frame update

    //bhop multiplier stuff
    [Header("Movment Related")]
    public Movement movementScript;
    public GameObject multiplierText;
    public int bhopCounter;
    private TextMeshProUGUI moveCounterText;

    //Health stuff
    [Header("Health Related")]
    public GameObject healthTextObj;
    public PlayerHealth health;
    public HealthBarScript healthBar;
    private TextMeshProUGUI healthTextGUI;

    [Header ("Grapple Cooldown Related")]
    public GameObject Grappleobj;
    public Movement Move;
    public Slider Grapple;
    private TextMeshProUGUI GrappleText;
    public GameObject canGrappleUI;

    [Header("Revolver Related")]
    public GameObject Abilityobj;
    public RevolverMovement revolverMovement;
    public Slider Ability;
    private TextMeshProUGUI AbilityText;

    [Header("Objective & Hitmarker Related")]
    public GameObject objTextObj;
    private TextMeshProUGUI objectiveText;
    public LevelHandler lvlHandler;
    public GameObject hitmarker;

    void Start()
    {
        healthTextGUI = healthTextObj.GetComponent<TextMeshProUGUI>();
        gameSpeedText = speedText.GetComponent<TextMeshProUGUI>();
        gameTimerText = timerText.GetComponent<TextMeshProUGUI>();
        moveCounterText = multiplierText.GetComponent<TextMeshProUGUI>();
        GrappleText = Grappleobj.GetComponent<TextMeshProUGUI>();
        AbilityText = Abilityobj.GetComponent<TextMeshProUGUI>();
        updateHealthUI(health.playerCurrentHealth);
        objectiveText = objTextObj.GetComponent<TextMeshProUGUI>();
        timeAdditionText = timerText.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        SetUpModifiers();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            unlimitedTime = unlimitedTime ? false : true;
        }
        gameSpeedText.text = player.velocity.magnitude.ToString("0.##") + " M|S";
        //check if timer is on
        if (timerOn)
        {
            bhopCounter = movementScript.bHopCount;
            changeBhopText(bhopCounter);

            if (!unlimitedTime)
            {
                timeLeft -= Time.deltaTime;
                timeLeft = timeLeft > -1 ? timeLeft : -1f;
                timerOn = timeLeft != -1 ? true : false;
                updateTimerText(timeLeft);
                if (timeLeft <= warnPlayerOfTime)
                {
                    changeTextColor(gameTimerText, timeLeft);
                }
            }
            /*timeLeft -= Time.deltaTime;
            timeLeft = timeLeft > -1 ? timeLeft : -1f;
            timerOn = timeLeft != -1 ? true : false;
            updateTimerText(timeLeft);
            if (timeLeft <= warnPlayerOfTime)
            {
                changeTextColor(gameTimerText, timeLeft);
            }*/
        }

        Grapple.value = 1f - (Move.actualGrappleCooldown / Move.grappleCooldown);
        if (Grapple.value == 1){
            GrappleText.text = "Ready";
        }
        else{
            GrappleText.text = Mathf.CeilToInt(Move.actualGrappleCooldown).ToString();
        }

        Ability.value = 1f - (revolverMovement.actualAbilityCooldown / revolverMovement.abilityCooldown);
        if (Ability.value == 1){
            AbilityText.text = "Ready";
        }
        else{
            AbilityText.text = Mathf.CeilToInt(revolverMovement.actualAbilityCooldown).ToString();
        }

        objectiveText.text = lvlHandler.enemiesKilled.ToString() + " | " + lvlHandler.enemiesToKill.ToString() + "  Slain";

        if(Move.canGrapple && Move.actualGrappleCooldown == 0){
            canGrappleUI.SetActive(true);
        }
        else{
            canGrappleUI.SetActive(false);
        }
    }

    void updateTimerText(float currentTime)             // 80
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
        gameTimerText.text = minutes.ToString() + ":" + secStr + milliStr;        
    }

    public void AddTime(float amount)
    {
        timeLeft += amount;
        timeAdditionText.text = "+ " + amount.ToString() + " S";
        StartCoroutine(FadeTextToZeroAlpha(2f, timeAdditionText));
    }

    void changeBhopText(int count)
    {
        moveCounterText.text = "x " + count.ToString() + " b-Hop Chain";
    }

    public void updateHealthUI(int n)
    {
        healthBar.setSliderHealth(n);
        healthTextGUI.text = n.ToString();
    }

    public void DisplayHitmarker(string hitboxName)
    {
        int index = (hitboxName == "CritHitbox" ? 1 : 0);
        StartCoroutine(FadeImageToZeroFrom(.3f, hitmarker.transform.GetChild(index).gameObject.transform.GetComponent<Image>(), .5f));
    }

    public IEnumerator FadeImageToZeroFrom(float startAlpha, Image i, float t)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, startAlpha);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
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

    public void changeTextColor( TextMeshProUGUI textmeshproGUI, float t)
    {   
        if ((int)t % 2  == 0)
        {
            textmeshproGUI.color = Color.white;
        }
        else
        {
            textmeshproGUI.color= Color.red;
        }
        
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
