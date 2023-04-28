using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboPlayerTriggers : MonoBehaviour
{
    
    public LimboSpawns limbospawn;

    void Update(){
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            limbospawn.SpawnTargets2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            limbospawn.SpawnTargets3();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)){
            limbospawn.SpawnFinalGrappleArea();
        }
    }

    void OnCollisionEnter(Collision col){
        if (col.gameObject.name == "HealthDropDummy"){
            limbospawn.SpawnWeaponSelectArea();
        }
        else if (col.gameObject.name == "RevolverDummy" || col.gameObject.name == "RocketDummy" || col.gameObject.name == "SniperDummy"){
            limbospawn.SpawnTargets1();
        }
        else if (col.gameObject.name == "EndPoint"){
            limbospawn.DespawnEnvironment();
        }
    }
    
}
