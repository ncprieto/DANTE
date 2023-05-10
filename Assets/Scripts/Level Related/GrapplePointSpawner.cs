using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePointSpawner : MonoBehaviour
{
    public GameObject grapplePoint;
    public float spawnTime;
    public List<Transform> locations;
    private int prevSpawn;

    void Start()
    {
        StartCoroutine(WaitForThenSpawn(spawnTime, grapplePoint));
    }

    public void SpawnNewGrapplePoint()
    {
        grapplePoint.SetActive(false);
        StartCoroutine(WaitForThenSpawn(spawnTime, grapplePoint));
    }

    private IEnumerator WaitForThenSpawn(float seconds, GameObject obj)
    {
        yield return new WaitForSeconds(seconds);
        int newSpawn = Random.Range(0, locations.Count);
        while(newSpawn != prevSpawn) newSpawn = Random.Range(0, locations.Count);
        obj.transform.position = locations[newSpawn].position;
        obj.SetActive(true);
        prevSpawn = newSpawn;
    }
}
