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
    public GameObject timeRingSpawns;

    private UnityEngine.Object timeRing;
    private UnityEngine.Object enemySpawnParticles;
    private int timeRingSpawnerCount;
    private int prevTimeRingLoc;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        timeRing = Resources.Load("Prefabs/TimeRing");
        enemySpawnParticles = Resources.Load("Prefabs/EnemySpawnParticles");
        timeRingSpawnerCount = timeRingSpawns.transform.childCount;
        enemiesKilled = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (enemiesKilled >= enemiesToKill){
            // win / next scene
        }
        //Debug.Log("base spawn count: " + timeRingSpawnerCount + "       current spawn count: " + timeRingSpawns.transform.childCount);
        if (timeRingSpawnerCount == timeRingSpawns.transform.childCount){
            Instantiate(timeRing, timeRingSpawns.transform.GetChild(FindTimeRingSpawn(timeRingSpawns, prevTimeRingLoc)).position, Quaternion.identity, timeRingSpawns.transform);
        }
    }

    public void SpawnEnemy(UnityEngine.Object enemy, Transform spawnPoint)
    {
        if (enemyHolder.transform.childCount < maxEnemyCount){
            StartCoroutine(SpawnTime(enemy, spawnPoint, 1f));
        }
    }

    int FindTimeRingSpawn(GameObject timeRingSpawner, int prevSpawn)
    {
        int generatedSpawn = Random.Range(0, timeRingSpawner.transform.childCount - 1);
        if (generatedSpawn != prevSpawn){
            prevSpawn = generatedSpawn;
            return generatedSpawn;
        }
        else{
            FindTimeRingSpawn(timeRingSpawner, prevSpawn);
        }
        return generatedSpawn;
    }

    IEnumerator SpawnTime(UnityEngine.Object enemy, Transform spawnPoint, float timeTillSpawn){
        Instantiate(enemySpawnParticles, spawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(timeTillSpawn);
        Instantiate(enemy, spawnPoint.position, Quaternion.identity, enemyHolder.transform);
    }
}
