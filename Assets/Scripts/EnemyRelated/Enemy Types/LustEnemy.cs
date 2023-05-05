using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LustEnemy : Enemy
{
    [Header ("Lust Specfic Variables")]
    public NavMeshAgent nmAgent;

    public Animator anims;

    // Patrol state vars
    public Vector3 patrolTo;
    bool patrolPointSet;
    public float patrolDistance;

    // State variables
    public float stalkRange;
    public float chaseRange;

    public float walkSpeed;
    public float stalkSpeed;
    public float chaseSpeed;

    private bool inStalkRange;
    private bool inChaseRange;
    private bool playerHit;
    private Vector3 shotDirection;

    // Update is called once per frame
    void Update()
    {
        // State range checks
        inStalkRange = Physics.CheckSphere(transform.position, stalkRange, playerLayer);
        inChaseRange = Physics.CheckSphere(transform.position, chaseRange, playerLayer);

        if (nmAgent.isOnOffMeshLink || nmAgent.speed == 0){
            anims.SetBool("isStill", true);
        }
        else if (!damageKnockback || !nmAgent.isOnOffMeshLink){
            anims.SetBool("isStill", false);
        }

        if (damageKnockback){
            DamageKnockbackState();
        }
        else if (antiStuck.pushBackEnemies){
            AntiStuckEnemyPushbackState();
        }
        else if (playerHit){
            HitPlayerState();
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
        anims.speed = 1f;
        nmAgent.speed = stalkSpeed;
        nmAgent.SetDestination(player.transform.position);
    }

    // Chase state functions
    void ChaseState()
    {
        anims.speed = 2f;
        nmAgent.speed = chaseSpeed;
        nmAgent.SetDestination(player.transform.position);
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
        nmAgent.velocity = -(transform.forward * 7.5f);
        nmAgent.speed = 5;
        transform.LookAt(player.transform);
        anims.SetBool("isStill", true);
    }

    // Send damage to player
    void OnCollisionEnter(Collision col){
        if (col.gameObject.tag == "Player"){
            playerHP.ReceiveDamage(10, true);
            StartCoroutine(HitPlayerStateTimer());
        }
    }

    IEnumerator HitPlayerStateTimer()
    {
        playerHit = true;
        yield return new WaitForSeconds(1.5f);
        playerHit = false;
    }
}
