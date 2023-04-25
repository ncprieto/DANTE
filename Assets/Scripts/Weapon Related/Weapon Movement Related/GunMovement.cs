using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMovement : MonoBehaviour
{
    private GameObject player;
    private Rigidbody rb;
    private Camera playerCamera;
    private Movement movement;
    private GunAttributes gunAttributes;

    public void Initialize(GameObject playerObj, GunAttributes ga)
    {
        player = playerObj;
        rb = player.GetComponent<Rigidbody>();
        movement = player.GetComponent<Movement>();
        gunAttributes = ga;
    }

    public virtual void ReceiveHitInfo(RaycastHit hit)
    {
        // overwritten
    }
}
