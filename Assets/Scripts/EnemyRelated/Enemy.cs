using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header ("Base Class Variables")]
    public float startingHealth;
    public LayerMask groundLayer;
    public LayerMask playerLayer;

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
}
