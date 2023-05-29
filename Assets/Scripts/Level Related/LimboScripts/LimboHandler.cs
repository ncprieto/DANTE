using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboHandler : MonoBehaviour
{

    public int currentLimboObj;
    public bool objChanged;

    [Header ("Game Objects")]
    public GameObject revolverObj;

    [Header ("UI Scripts")]
    public Movement movementScript;
    public TimeUpdater timeUpdaterScript;

    [Header ("Waypoint Positions")]
    public Transform from1;
    public Transform from2;
    public Transform from3_1;
    public Transform from3_2;
    public Transform from3_3;
    public Transform from4_1;
    public Transform from4_2;
    public Transform from5;

    void Start()
    {
        currentLimboObj = 1;
        objChanged = true;
        movementScript.DisableGrappleUI();
        movementScript.DisableBHopUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (objChanged){
            objChanged = false;
            movementScript.StopGrapple();
            switch (currentLimboObj){
                case 1:
                    transform.position = from1.position;
                    break;
                case 2:
                    transform.position = from2.position;
                    movementScript.EnableBHopUI();
                    break;
                case 3:
                    transform.position = from3_1.position;
                    movementScript.EnableGrappleUI();
                    movementScript.grappleEnabled = true;
                    break;
                case 4:
                    transform.position = from3_2.position;
                    break;
                case 5:
                    transform.position = from3_3.position;
                    break;
                case 6:
                    transform.position = from4_1.position;
                    revolverObj.SetActive(true);
                    break;
                case 7:
                    transform.position = from4_2.position;
                    break;
                case 8:
                    transform.position = from5.position;
                    timeUpdaterScript.enabled = true;
                    break;
                case 9:
                    transform.position = new Vector3(0, 0, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
