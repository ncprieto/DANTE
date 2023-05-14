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
    // public GameObject largeTimeRingSpawns;
    // public GameObject smallTimeRingSpawns;

    // private UnityEngine.Object largeTimeRing;
    // private UnityEngine.Object smallTimeRing;
    private UnityEngine.Object enemySpawnParticles;
    // private int largeTimeRingSpawnerCount;
    // private int prevLargeTimeRingLoc;
    // private int smallTimeRingSpawnerCount;
    // private int prevSmallTimeRingLoc;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // largeTimeRing = Resources.Load("Prefabs/LargeTimeRing");
        // smallTimeRing = Resources.Load("Prefabs/SmallTimeRing");
        enemySpawnParticles = Resources.Load("Prefabs/EnemySpawnParticles");
        // largeTimeRingSpawnerCount = largeTimeRingSpawns.transform.childCount;
        // smallTimeRingSpawnerCount = smallTimeRingSpawns.transform.childCount;
        enemiesKilled = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (enemiesKilled >= enemiesToKill){
            // win / next scene
        }
        //Debug.Log("base spawn count: " + largeTimeRingSpawnerCount + "       current spawn count: " + largeTimeRingSpawns.transform.childCount);
        // if (largeTimeRingSpawnerCount == largeTimeRingSpawns.transform.childCount){
        //     Instantiate(largeTimeRing, largeTimeRingSpawns.transform.GetChild(FindTimeRingSpawn(largeTimeRingSpawns, prevLargeTimeRingLoc)).position, Quaternion.identity, largeTimeRingSpawns.transform);
        // }
    }

    public void SpawnEnemy(UnityEngine.Object enemy, Transform spawnPoint)
    {
        if (enemyHolder.transform.childCount < maxEnemyCount){
            StartCoroutine(SpawnTime(enemy, spawnPoint, 1f));
        }
    }

    // int FindTimeRingSpawn(GameObject largeTimeRingSpawner, int prevSpawn)
    // {
    //     int generatedSpawn = Random.Range(0, largeTimeRingSpawner.transform.childCount - 1);
    //     if (generatedSpawn != prevSpawn){
    //         prevSpawn = generatedSpawn;
    //         return generatedSpawn;
    //     }
    //     else{
    //         FindTimeRingSpawn(largeTimeRingSpawner, prevSpawn);
    //     }
    //     return generatedSpawn;
    // }

    IEnumerator SpawnTime(UnityEngine.Object enemy, Transform spawnPoint, float timeTillSpawn){
        Instantiate(enemySpawnParticles, spawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(timeTillSpawn);
        Instantiate(enemy, spawnPoint.position, Quaternion.identity, enemyHolder.transform);
    }
}
