using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireAreaMove : MonoBehaviour
{

    public NavMeshAgent nmAgent;
    public LayerMask groundLayer;

    // Patrol state vars
    public Vector3 patrolTo;
    private bool patrolPointSet;
    private bool randomTimeChosen;
    private bool isWaiting;
    public float patrolDistance;

    // Start is called before the first frame update
    void Start()
    {
        patrolPointSet = false;
        randomTimeChosen = false;
        isWaiting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaiting){
            WaitState();
        }
        else if (!isWaiting){
            PatrolState();
        }
    }

    void PatrolState(){
        if (!patrolPointSet){
            FindPatrolPoint();
        }

        if (patrolPointSet){
            nmAgent.SetDestination(patrolTo);
        }

        Vector3 distFromPatrolPoint = transform.position - patrolTo;
        if (distFromPatrolPoint.magnitude < 1f){
            patrolPointSet = false;
            isWaiting = true;
            randomTimeChosen = false;
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

    void WaitState(){
        if (!randomTimeChosen){
            StartCoroutine(WaitToPatrol());
        }
    }

    IEnumerator WaitToPatrol(){
        randomTimeChosen = true;
        float rand = Random.Range(0.5f, 5f);
        yield return new WaitForSeconds(rand);
        isWaiting = false;
    }
}
