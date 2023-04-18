using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboSpawns : MonoBehaviour
{

    public void SpawnWeaponSelectArea(){
        if (!gameObject.transform.GetChild(1).gameObject.activeSelf){
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void SpawnTargets1(){
        if (!gameObject.transform.GetChild(2).gameObject.activeSelf){
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    public void SpawnTargets2(){
        if (!gameObject.transform.GetChild(3).gameObject.activeSelf){
            gameObject.transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    public void SpawnTargets3(){
        if (!gameObject.transform.GetChild(4).gameObject.activeSelf){
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
        }
    }

    public void SpawnFinalGrappleArea(){
        if (!gameObject.transform.GetChild(5).gameObject.activeSelf){
            gameObject.transform.GetChild(5).gameObject.SetActive(true);
        }
    }

    public void DespawnEnvironment(){
        gameObject.SetActive(false);
    }

}
