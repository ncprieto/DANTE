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
    private int timeRingSpawnerCount;
    private int prevTimeRingLoc;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        timeRing = Resources.Load("Prefabs/TimeRing");
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
            Instantiate(enemy, spawnPoint.position, Quaternion.identity, enemyHolder.transform);
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
}
