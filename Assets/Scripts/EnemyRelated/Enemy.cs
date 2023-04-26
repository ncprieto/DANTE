using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header ("Base Class Variables")]
    public float startingHealth;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public TimeValues timeValues;

    protected GameObject player;
    protected PlayerHealth playerHP;

    void Awake()
    {
        player = GameObject.Find("Player");
        playerHP = player.GetComponent<PlayerHealth>();
    }

    void Start()
    {
        currentHealth = startingHealth;
    }

    // Health System Related Functions
    protected float currentHealth;
    public void ReceiveDamage(float dmg)
    {
        if(currentHealth > 0) currentHealth -= dmg;
        if(currentHealth <= 0) Destroy(this.gameObject);
    }

    public void GiveHealth(float amount)
    {
        if(currentHealth < startingHealth) currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, startingHealth);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool IsThisDamageLethal(float amount)
    {
        return currentHealth - amount <= 0;
    }

    // Time Value For Enemy Specific
    public float GetTimeRewardValue(string hitbox)
    {
        return timeValues.GetRewardValue(hitbox);
    }
}
