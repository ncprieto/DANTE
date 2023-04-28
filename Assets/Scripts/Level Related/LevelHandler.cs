using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{

    [Header ("Base Class Variables")]
    public int enemiesToKill;
    public int enemiesKilled;
    public int maxEnemyCount;
    public GameObject enemyHolder;

    // Start is called before the first frame update
    void Start()
    {
        enemiesKilled = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesKilled >= enemiesToKill){
            // win / next scene
        }
    }

    public void SpawnEnemy(UnityEngine.Object enemy, Transform spawnPoint)
    {
        if (enemyHolder.transform.childCount < maxEnemyCount){
            Instantiate(enemy, spawnPoint.position, Quaternion.identity, enemyHolder.transform);
        }
    }
}
