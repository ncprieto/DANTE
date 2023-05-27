using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboHandler : MonoBehaviour
{

    public int currentLimboObj = 1;

    public Transform from1;
    public Transform from2;
    public Transform from3_1;
    public Transform from3_2;
    public Transform from3_3;
    public Transform from4_1;
    public Transform from4_2;
    public Transform from5;

    // Update is called once per frame
    void Update()
    {
        switch (currentLimboObj){
            case 1:
                transform.position = from1.position;
                break;
            case 2:
                transform.position = from2.position;
                break;
            case 3:
                transform.position = from3_1.position;
                break;
            case 4:
                transform.position = from3_2.position;
                break;
            case 5:
                transform.position = from3_3.position;
                break;
            case 6:
                transform.position = from4_1.position;
                break;
            case 7:
                transform.position = from4_2.position;
                break;
            case 8:
                transform.position = from5.position;
                break;
            case 9:
                transform.position = new Vector3(0, 0, 0);
                break;
            default:
                break;
        }
    }
}
