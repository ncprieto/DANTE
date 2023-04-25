using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Script : MonoBehaviour
{
    public Rigidbody player;
    public GameObject speedText;

    //timer stuff
    [Header("Time Related")]
    public GameObject timerText;
    public bool timerOn;
    public float timeLeft;

    //in game speed component
    TextMeshProUGUI gameSpeedText;
    TextMeshProUGUI gameTimerText;
    // Start is called before the first frame update

    //bhop multiplier stuff
    [Header("Movment Related")]
    public Movement movementScript;
    public GameObject multiplierText;
    public int bhopCounter;
    TextMeshProUGUI moveCounterText;

    //Health stuff
    [Header("Health Related")]
    public GameObject healthTextObj;
    TextMeshProUGUI healthTextGUI;

    public PlayerHealth health;

    public HealthBarScript healthBar;

    void Start()
    {
        healthTextGUI = healthTextObj.GetComponent<TextMeshProUGUI>();
        gameSpeedText = speedText.GetComponent<TextMeshProUGUI>();
        gameTimerText = timerText.GetComponent<TextMeshProUGUI>();
        moveCounterText = multiplierText.GetComponent<TextMeshProUGUI>();
        updateHealthUI(health.playerCurrentHealth);

    }

    // Update is called once per frame
    void Update()
    {
        gameSpeedText.text = player.velocity.magnitude.ToString("0.##") + " m/s";

        //check if timer is on
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                bhopCounter = movementScript.bHopCount;
                if (bhopCounter > 0)
                {
                    changeBhopText(bhopCounter);
                }
                else
                {
                    multiplierText.SetActive(false);
                }
                timeLeft -= Time.deltaTime;
                updateTimerText(timeLeft);
            }
            else 
            { 
                timeLeft = 0;
                timerOn = false;
            }
        }
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
}
