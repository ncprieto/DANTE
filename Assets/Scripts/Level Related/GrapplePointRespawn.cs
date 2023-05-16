using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePointRespawn : MonoBehaviour
{

    public float timeTillRespawn;
    public GameObject hitbox;
    public ParticleSystem respawnParticles;
    public ParticleSystem despawnParticles;

    public bool despawn;

    // Start is called before the first frame update
    void Start()
    {
        despawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (despawn){
            despawn = false;
            hitbox.SetActive(false);
            despawnParticles.Play();
            StartCoroutine(RespawnGrapplePoint(timeTillRespawn));
            float respawnPTime = timeTillRespawn - 3f;
            if (respawnPTime < 0f) respawnPTime = 0f;
            StartCoroutine(RespawnGrappleParticles(respawnPTime));
        }
    }

    IEnumerator RespawnGrapplePoint(float respawnTime){
        yield return new WaitForSeconds(respawnTime);
        hitbox.SetActive(true);
    }

    IEnumerator RespawnGrappleParticles(float respawnParticlesTime){
        yield return new WaitForSeconds(respawnParticlesTime);
        respawnParticles.Play();
    }
}
