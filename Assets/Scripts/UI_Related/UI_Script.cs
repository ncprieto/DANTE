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
    }

    // Update is called once per frame
    void Update()
    {
        gameSpeedText.text = player.velocity.magnitude.ToString("0.##") + " m/s";
        //check if timer is on
        if (timerOn)
        {
            bhopCounter = movementScript.bHopCount;
            if (bhopCounter > 0) changeBhopText(bhopCounter);
            else multiplierText.SetActive(false);

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

        objectiveText.text = "Demons Slain: " + lvlHandler.enemiesKilled.ToString() + " / " + lvlHandler.enemiesToKill.ToString();

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
        gameTimerText.text = minutes.ToString() + " : " + secStr + milliStr;        
    }

    public void AddTime(float amount)
    {
        timeLeft += amount;
        timeAdditionText.text = "+ " + amount.ToString() + "s";
        StartCoroutine(FadeTextToZeroAlpha(2f, timeAdditionText));
    }

    void changeBhopText(int count)
    {
        moveCounterText.text = "x" + count.ToString() + " bHop Chain";
        multiplierText.SetActive(true);
    }

    public void updateHealthUI(int n)
    {
        healthBar.setSliderHealth(n);
        healthTextGUI.text = n.ToString();
    }

    public IEnumerator DisplayHitmarker(string hitboxName)
    {
        int index = (hitboxName == "CritHitbox" ? 1 : 0);
        hitmarker.transform.GetChild(index).gameObject.SetActive(true);
        yield return new WaitForSeconds(.1f);
        hitmarker.transform.GetChild(index).gameObject.SetActive(false);
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
            textmeshproGUI.color= Color.black;
        }
        
    }
}
