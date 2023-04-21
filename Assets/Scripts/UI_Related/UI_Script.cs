using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Script : MonoBehaviour
{
    public Rigidbody player;
    public GameObject speedText;
    
    //timer stuff
    public GameObject timerText;
    public bool timerOn;
    public float timeLeft;

    //in game speed component
    TextMeshProUGUI gameSpeedText;
    TextMeshProUGUI gameTimerText;
    // Start is called before the first frame update
    void Start()
    {
        gameSpeedText = speedText.GetComponent<TextMeshProUGUI>();
        gameTimerText = timerText.GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        gameSpeedText.text = player.velocity.magnitude.ToString("0.##");

        //check if timer is on
        if (timerOn)
        {
            if (timeLeft > 0)
            {
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
}
