using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LustEnemy : Enemy
{
    [Header ("Lust Specfic Variables")]
    public NavMeshAgent nmAgent;
    public Animator anims;
    public float damagePerHit;

    // Patrol state vars
    public Vector3 patrolTo;
    public bool patrolPointSet;
    public float patrolDistance;

    // State variables
    public float stalkRange;
    public float chaseRange;

    public float walkSpeed;
    public float stalkSpeed;
    public float chaseSpeed;
    private float origAccel;

    private bool inStalkRange;
    private bool inChaseRange;
    private bool playerHit;
    private Vector3 shotDirection;

    private float stalkRotation;
    private float stalkRandTime;
    private float stalkRotTimer;
    private bool enterScatterState;
    private float timeTilSFX;

    protected override void Start()
    {
        base.Start();
        timeTilSFX = Random.Range(2f, 10f);
        origAccel = nmAgent.acceleration;
        patrolPointSet = false;
        stalkRotation = 0f;
        stalkRandTime = Random.Range(.5f, 3f);
        stalkRotTimer = stalkRandTime;
        enterScatterState = false;
        SetUpModifiers();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // State range checks
        inStalkRange = Physics.CheckSphere(transform.position, stalkRange, playerLayer);
        inChaseRange = Physics.CheckSphere(transform.position, chaseRange, playerLayer);

        // Fellow enemies nearby check
        if (otherEnemiesInRange > 3 && !enterScatterState){
            enterScatterState = true;
            StartCoroutine(ScatterStateTimer());
        }

        // Correct anim plays
        if (nmAgent.isOnOffMeshLink || nmAgent.speed == 0){
            anims.SetBool("isStill", true);
        }
        else if (!damageKnockback || !nmAgent.isOnOffMeshLink){
            anims.SetBool("isStill", false);
        }

        // State changes
        if (damageKnockback){
            DamageKnockbackState();
        }
        else if (antiStuck.pushBackEnemies){
            AntiStuckEnemyPushbackState();
        }
        else if (playerHit){
            HitPlayerState();
        }
        else if (enterScatterState){
            ScatterState();
        }
        else if (!inStalkRange && !inChaseRange){
            PatrolState();
        }
        else if (inStalkRange && !inChaseRange){
            StalkState();
        }
        else if (inStalkRange && inChaseRange){
            ChaseState();
        }
    }

    // Patrol state functions
    void PatrolState()
    {
        anims.speed = .5f;
        nmAgent.speed = walkSpeed;

        if (!patrolPointSet){
            FindPatrolPoint();
        }

        if (patrolPointSet){
            nmAgent.SetDestination(patrolTo);
        }

        Vector3 distFromPatrolPoint = transform.position - patrolTo;
        if (distFromPatrolPoint.magnitude < 1f){
            patrolPointSet = false;
        }
    }

    void FindPatrolPoint()
    {
        float newX = Random.Range(-patrolDistance, patrolDistance);
        float newZ = Random.Range(-patrolDistance, patrolDistance);

        patrolTo = new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ);

        if (Physics.Raycast(patrolTo, -transform.up, 2f, groundLayer)){
            patrolPointSet = true;
        }
    }

    // Stalk state functions
    void StalkState()
    {
        stalkRotTimer += Time.deltaTime;
        if (stalkRotTimer >= stalkRandTime){
            stalkRotation = Random.value > 0.5f ? Random.Range(-30f, -5f) : Random.Range(5f, 30f);
            stalkRotTimer = 0f;
            stalkRandTime = Random.Range(.5f, 3f);
        }
        anims.speed = 1f;
        nmAgent.speed = stalkSpeed;
        nmAgent.SetDestination(Quaternion.AngleAxis(stalkRotation, Vector3.up) * player.transform.position);
    }

    // Chase state functions
    void ChaseState()
    {
        anims.speed = 2f;
        nmAgent.speed = chaseSpeed;
        nmAgent.SetDestination(player.transform.position);
        PlayRoarSFX();
    }

    void HitPlayerState()
    {
        anims.speed = 1f;
        nmAgent.speed = 0;
    }

    void DamageKnockbackState()
    {
        anims.speed = 1f;
        if (!invertVelocity){
            shotDirection = Camera.main.transform.forward * 5;
            shotDirection.y = nmAgent.velocity.y;
            invertVelocity = true;
        }
        nmAgent.velocity = shotDirection;
        nmAgent.speed = 5;
        transform.LookAt(player.transform);
        anims.SetBool("isStill", true);
    }

    void AntiStuckEnemyPushbackState()
    {
        anims.speed = 1f;
        nmAgent.speed = 5;
        nmAgent.velocity = -(transform.forward * 10f);
        transform.LookAt(player.transform);
        anims.SetBool("isStill", true);
    }

    void ScatterState()
    {
        anims.speed = 1f;
        nmAgent.speed = stalkSpeed;

        if (!patrolPointSet){
            FindPatrolPoint();
        }

        if (patrolPointSet){
            nmAgent.SetDestination(patrolTo);
        }

        Vector3 distFromPatrolPoint = transform.position - patrolTo;
        if (distFromPatrolPoint.magnitude < 1f){
            patrolPointSet = false;
        }

    }

    bool waitingForSFX;
    void PlayRoarSFX()
    {
        if(waitingForSFX) return;
        StartCoroutine(WaitForThenPlaySFX());
    }

    IEnumerator WaitForThenPlaySFX()
    {
        waitingForSFX = true;
        yield return new WaitForSeconds(timeTilSFX);
        mainSFXEvent.Play();
        timeTilSFX = Random.Range(2f, 10f);
        waitingForSFX = false;
    }

    IEnumerator ScatterStateTimer()
    {
        yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
        enterScatterState = false;
    } 

    // Send damage to player
    void OnCollisionEnter(Collision col){
        if (col.gameObject.tag == "Player"){
            playerHP.ReceiveDamage((int)damagePerHit, true);
            StartCoroutine(HitPlayerStateTimer());
        }
    }

    IEnumerator HitPlayerStateTimer()
    {
        playerHit = true;
        yield return new WaitForSeconds(1.5f);
        playerHit = false;
    }

    protected override void SetUpModifiers()
    {
        ApplyModifier("Enemy Speed",  ref walkSpeed);
        ApplyModifier("Enemy Speed",  ref stalkSpeed);
        ApplyModifier("Enemy Speed",  ref chaseSpeed);
    }
}
