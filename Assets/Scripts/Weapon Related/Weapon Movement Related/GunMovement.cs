using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMovement : MonoBehaviour
{
    protected GameObject player;
    protected Rigidbody rb;
    protected Camera playerCamera;
    protected Movement movement;
    protected GunAttributes gunAttributes;
    protected FOVVFX fovVFX;

    public void Initialize(GameObject playerObj, GunAttributes ga)
    {
        player = playerObj;
        rb = player.GetComponent<Rigidbody>();
        movement = player.GetComponent<Movement>();
        fovVFX = player.GetComponent<FOVVFX>();
        gunAttributes = ga;
    }

    public virtual void ReceiveHitInfo(RaycastHit hit)
    {
        // overwritten
    }
}
