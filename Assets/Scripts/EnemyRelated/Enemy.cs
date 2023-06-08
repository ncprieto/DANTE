using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Enemy : MonoBehaviour
{
    [Header ("Base Class Variables")]
    public float startingHealth;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public TimeValues timeValues;
    public bool isTarget;

    [Header ("SFX Events")]
    public FMODUnity.StudioEventEmitter mainSFXEvent;
    public FMODUnity.StudioEventEmitter deathSFXEvent;

    protected GameObject player;
    protected PlayerHealth playerHP;
    protected AntiStuck antiStuck;
    protected LevelHandler lvlHandler;
    protected GameObject healthDrops;
    protected LimboHandler limboHandler;

    private UnityEngine.Object hpDrop;
    private UnityEngine.Object deathParticles;

    void Awake()
    {
        player = GameObject.Find("Player");
        playerHP = player.GetComponent<PlayerHealth>();
        if (!isTarget){
            antiStuck = player.transform.GetChild(2).gameObject.GetComponent<AntiStuck>();
            lvlHandler = GameObject.Find("LevelHandler").GetComponent<LevelHandler>();
            healthDrops = GameObject.Find("HealthDrops");
        }
        else{
            limboHandler = GameObject.Find("LimboWaypoint").GetComponent<LimboHandler>();
        }
    }

    protected virtual void Start()
    {
        currentHealth = startingHealth;
        if (!isTarget){
            otherEnemiesInRange = 0;
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
            deathParticles = Resources.Load("Prefabs/NewEnemyDeathParticles");
        }
        else{
            deathParticles = Resources.Load("Prefabs/TargetDeathParticles");
        }
    }

    protected static bool enemyHasDied;
    public int otherEnemiesInRange;
    protected virtual void Update()
    {
        if (!isTarget){
            if (otherEnemiesInRange > 0 && enemyHasDied){
                otherEnemiesInRange--;
            }
            enemyHasDied = false;
        }
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
            if (!isTarget){
                Instantiate(hpDrop, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.position.z), Quaternion.identity, healthDrops.transform);
                Instantiate(deathParticles, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 3, this.gameObject.transform.position.z), Quaternion.identity);
                enemyHasDied = true;
                lvlHandler.enemiesKilled++;
                deathSFXEvent.Play();
            }
            else{
                Instantiate(deathParticles, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity);
                limboHandler.targetsDestroyed++;
            }
            Destroy(this.gameObject);
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

    protected virtual void SetUpModifiers()
    {
        // overwritten
    }

    protected void ApplyModifier(string modifierName, ref float value)
    {
        value *= PlayerPrefs.GetFloat(modifierName, 1);
    }

    protected void ApplyModifier(string modifierName, ref int value)
    {
        value *= (int)PlayerPrefs.GetFloat(modifierName, 1);
    }
}
