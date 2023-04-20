using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoMove : MonoBehaviour
{

    public Transform player;

    public float moveSpeed;
    public float spinSpeed;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);

    }
}
