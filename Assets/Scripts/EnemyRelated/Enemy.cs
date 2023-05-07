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
    protected AntiStuck antiStuck;
    protected LevelHandler lvlHandler;
    protected GameObject healthDrops;

    private UnityEngine.Object hpDrop;
    private UnityEngine.Object deathParticles;

    void Awake()
    {
        player = GameObject.Find("Player");
        playerHP = player.GetComponent<PlayerHealth>();
        antiStuck = player.transform.GetChild(2).gameObject.GetComponent<AntiStuck>();
        lvlHandler = GameObject.Find("LevelHandler").GetComponent<LevelHandler>();
        healthDrops = GameObject.Find("HealthDrops");
    }

    protected virtual void Start()
    {
        currentHealth = startingHealth;
        int hpDropChance = Random.Range(0, 100);
        if (hpDropChance < 50){
            hpDrop = Resources.Load("Prefabs/SmallHealthDrop");
        }
        else if (hpDropChance < 85){
            hpDrop = Resources.Load("Prefabs/MediumHealthDrop");
        }
        else{
            hpDrop = Resources.Load("Prefabs/LargeHealthDrop");
        }
        deathParticles = Resources.Load("Prefabs/EnemyDeathParticles");
    }

    // Health System Related Functions
    protected float currentHealth;
    protected bool damageKnockback;
    protected bool invertVelocity = false;
    public void ReceiveDamage(float dmg)
    {
        StartCoroutine(DamageKnockbackStateTimer());
        if (currentHealth > 0) currentHealth -= dmg;
        if (currentHealth <= 0){
            Instantiate(hpDrop, new Vector3 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.position.z), Quaternion.identity, healthDrops.transform);
            Instantiate(deathParticles, new Vector3 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + 3, this.gameObject.transform.position.z), Quaternion.identity);
            Destroy(this.gameObject);
            lvlHandler.enemiesKilled++;
        }
    }

    public void GiveHealth(float amount)
    {
        if (currentHealth < startingHealth) currentHealth += amount;
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

    public void BloodParticles(Transform hitbox)
    {
        hitbox.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
    }

    IEnumerator DamageKnockbackStateTimer()
    {
        damageKnockback = true;
        yield return new WaitForSeconds(.75f);
        damageKnockback = false;
        invertVelocity = false;
    }
}
