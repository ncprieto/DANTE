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

    //in game speed component
    private TextMeshProUGUI gameSpeedText;
    private TextMeshProUGUI gameTimerText;
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

            timeLeft -= Time.deltaTime;
            timeLeft = timeLeft > -1 ? timeLeft : -1f;
            timerOn = timeLeft != -1 ? true : false;
            updateTimerText(timeLeft);
        }

        Grapple.value = 1f - (Move.actualGrappleCooldown / Move.grappleCooldown);
        GrappleText.text = Mathf.CeilToInt(Move.actualGrappleCooldown).ToString();

        Ability.value = 1f - (revolverMovement.actualAbilityCooldown / revolverMovement.abilityCooldown);
        AbilityText.text = Mathf.CeilToInt(revolverMovement.actualAbilityCooldown).ToString();

        objectiveText.text = "Demons Slain: " + lvlHandler.enemiesKilled.ToString() + " / " + lvlHandler.enemiesToKill.ToString();
    }

    void updateTimerText(float currentTime)             // 80
    {
        currentTime += 1;

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int milliseconds = Mathf.FloorToInt((currentTime % 60 * 1000) % 1000);

        //update the text     
        gameTimerText.text = minutes.ToString() + " : " + seconds.ToString() + "." + milliseconds.ToString();
    }

    public void AddTime(float amount)
    {
        timeLeft += amount;
    }

    void changeBhopText(int count)
    {
        moveCounterText.text = "X " + count.ToString() + " bHop Chain";
        multiplierText.SetActive(true);
    }

    public void updateHealthUI(int n)
    {
        healthBar.setSliderHealth(n);
        healthTextGUI.text = n.ToString();
    }

    public IEnumerator DisplayHitmarker()
    {
        hitmarker.SetActive(true);
        yield return new WaitForSeconds(.1f);
        hitmarker.SetActive(false);
    }
}
