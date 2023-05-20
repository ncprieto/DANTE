using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{

    [Header ("Base Class Variables")]
    public float enemiesToKill;
    public int enemiesKilled;
    public int maxEnemyCount;
    public GameObject enemyHolder;
    
    private UnityEngine.Object enemySpawnParticles;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        enemySpawnParticles = Resources.Load("Prefabs/EnemySpawnParticles");
        enemiesKilled = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (enemiesKilled >= (int)enemiesToKill){
            // win / next scene
        }
    }

    public void SpawnEnemy(UnityEngine.Object enemy, Transform spawnPoint)
    {
        if (enemyHolder.transform.childCount < maxEnemyCount){
            StartCoroutine(SpawnTime(enemy, spawnPoint, 1f));
        }
    }

    IEnumerator SpawnTime(UnityEngine.Object enemy, Transform spawnPoint, float timeTillSpawn){
        Instantiate(enemySpawnParticles, spawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(timeTillSpawn);
        Instantiate(enemy, spawnPoint.position, Quaternion.identity, enemyHolder.transform);
    }
}
