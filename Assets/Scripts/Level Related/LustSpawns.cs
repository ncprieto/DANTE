using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LustSpawns : LevelHandler
{
    [Header ("Lust Level Variables")]
    public float spawnRateTime;
    public int spawnRateKills;
    public int spawnAmount;

    private UnityEngine.Object lustEnemy;
    private float timer;
    private List<int> generatedSpawnPoints = new List<int>();
    private bool haveKillsSpawnsHappened;
    private int enemyCountPreCheck;

    // Start is called before the first frame update
    protected override void Start()
    {
        lustEnemy = Resources.Load("Prefabs/LustEnemy");
        timer = 0f;
        haveKillsSpawnsHappened = false;
        enemyCountPreCheck = enemyHolder.transform.childCount;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnRateTime){
            SpawnEnemy(lustEnemy, transform.GetChild(Random.Range(0, transform.childCount - 1)));
            timer = 0f;
        }
        if ((enemiesKilled % spawnRateKills == 0) && (enemiesKilled != 0) && (!haveKillsSpawnsHappened)){
            enemyCountPreCheck = enemyHolder.transform.childCount;
            for (int i = 0; i < spawnAmount; i++){
                enemyCountPreCheck++;
                if (enemyCountPreCheck < maxEnemyCount){
                    SpawnEnemy(lustEnemy, transform.GetChild(FindSpawnPoint()));
                }
            }
            generatedSpawnPoints.Clear();
            haveKillsSpawnsHappened = true;
        }
        if (enemiesKilled % spawnRateKills != 0){
            haveKillsSpawnsHappened = false;
        }
        base.Update();
    }

    int FindSpawnPoint()
    {
        int generatedSpawn = Random.Range(0, transform.childCount - 1);
        if (!generatedSpawnPoints.Contains(generatedSpawn)){
            generatedSpawnPoints.Add(generatedSpawn);
            return generatedSpawn;
        }
        else{
            FindSpawnPoint();
        }
        return generatedSpawn;
    }
}
