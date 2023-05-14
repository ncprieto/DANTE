using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRingSpawns : MonoBehaviour
{
    public bool isLarge;
    public float timeTillNextRing;

    public bool isRingActive;

    private UnityEngine.Object timeRing;
    private int timeRingSpawnerCount;
    private bool isGoingToSpawnRing;
    private int prevTimeRingLoc;

    // Start is called before the first frame update
    void Start()
    {
        if (isLarge){
            timeRing = Resources.Load("Prefabs/LargeTimeRing");
        }
        else{
            timeRing = Resources.Load("Prefabs/SmallTimeRing");
        }
        timeRingSpawnerCount = transform.childCount;
        isGoingToSpawnRing = false;
        prevTimeRingLoc = -1;
        isRingActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRingSpawnerCount == transform.childCount && !isGoingToSpawnRing){
            isGoingToSpawnRing = true;
            StartCoroutine(RingSpawnTime(timeRing, transform.GetChild(FindTimeRingSpawn(prevTimeRingLoc)), timeTillNextRing));
        }

        if (timeRingSpawnerCount == transform.childCount && isRingActive){
            isRingActive = false;
        }
        else if (timeRingSpawnerCount != transform.childCount && !isRingActive){
            isRingActive = true;
        }
    }

    int FindTimeRingSpawn(int prevSpawn)
    {
        int generatedSpawn = Random.Range(0, transform.childCount - 1);
        if (generatedSpawn != prevSpawn){
            prevSpawn = generatedSpawn;
            return generatedSpawn;
        }
        else{
            return FindTimeRingSpawn(prevSpawn);
        }
    }

    IEnumerator RingSpawnTime(UnityEngine.Object ring, Transform spawnPoint, float timeTillSpawn){
        yield return new WaitForSeconds(timeTillSpawn);
        Instantiate(ring, spawnPoint.position, spawnPoint.rotation, transform);
        isGoingToSpawnRing = false;
    }
}
