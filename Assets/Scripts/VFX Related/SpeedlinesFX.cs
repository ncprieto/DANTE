using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class SpeedlinesFX : MonoBehaviour
{

    public Rigidbody player;

    public float minSpeed;
    public float maxSpeed;

    public float maxParticles;

    private ParticleSystem thisSystem;

    // Start is called before the first frame update
    void Start()
    {
        thisSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var em = thisSystem.emission;
        em.rateOverTime = Mathf.Clamp(math.remap(minSpeed, maxSpeed, 0, maxParticles, player.velocity.magnitude), 0, maxParticles);
    }
}
