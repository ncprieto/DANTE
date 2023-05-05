using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHealth : MonoBehaviour
{

    public PlayerHealth playerHP;

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "SmallHealth" || col.gameObject.tag == "MediumHealth" || col.gameObject.tag == "LargeHealth"){
            playerHP.GainHealth(col.gameObject.tag);
            Destroy(col.gameObject);
        }
    }
}
