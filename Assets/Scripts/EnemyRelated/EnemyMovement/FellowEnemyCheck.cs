using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellowEnemyCheck : MonoBehaviour
{
    public Enemy enemyScript;

    // Fellow enemies in range checks
    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "FellowEnemy"){
            enemyScript.otherEnemiesInRange++;
        }
    }

    void OnTriggerExit(Collider col){
        if (col.gameObject.tag == "FellowEnemy"){
            if (enemyScript.otherEnemiesInRange > 0){
                enemyScript.otherEnemiesInRange--;
            }
        }
    }
}
