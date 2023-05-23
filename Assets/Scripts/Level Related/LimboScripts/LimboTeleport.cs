using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboTeleport : MonoBehaviour
{
    public Transform tpTo;
    public GameObject prevSegment;
    public GameObject nextSegment;

    private UnityEngine.Object tpParticles;

    // Start is called before the first frame update
    void Start()
    {
        tpParticles = Resources.Load("Prefabs/TeleportSmokeParticles");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col){
        if (col.gameObject.tag == "Player"){
            col.gameObject.transform.position = tpTo.position;
            Instantiate(tpParticles, col.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f), col.gameObject.transform);
            nextSegment.SetActive(true);
            prevSegment.SetActive(false);
        }
    }
}
