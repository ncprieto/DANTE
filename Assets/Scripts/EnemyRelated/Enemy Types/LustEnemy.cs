using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LustEnemy : Enemy
{
    [Header ("Lust Specfic Variables")]
    public NavMeshAgent nmAgent;

    // Patrol state vars
    public Vector3 patrolTo;
    bool patrolPointSet;
    public float patrolDistance;

    // State variables
    public float stalkRange;
    public float chaseRange;
    public bool inStalkRange;
    public bool inChaseRange;

    // Update is called once per frame
    void Update()
    {
        // State range checks
        inStalkRange = Physics.CheckSphere(transform.position, stalkRange, playerLayer);
        inChaseRange = Physics.CheckSphere(transform.position, chaseRange, playerLayer);

        if (!inStalkRange && !inChaseRange){
            PatrolState();
        }
        if (inStalkRange && !inChaseRange){
            StalkState();
        }
        if (inStalkRange && inChaseRange){
            ChaseState();
        }
    }

    // Patrol state functions
    void PatrolState()
    {
        nmAgent.speed = 3;

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
        nmAgent.speed = 6;
        nmAgent.SetDestination(player.transform.position);
    }

    // Chase state functions
    void ChaseState()
    {
        nmAgent.speed = 12;
        nmAgent.SetDestination(player.transform.position);
    }

    // Send damage to player
    void OnCollisionEnter(Collision col){
        if (col.gameObject.tag == "Player"){
            playerHP.ReceiveDamage(10, true);
        }
    }
}
