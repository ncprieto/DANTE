using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiStuck : MonoBehaviour
{

    public int enemiesNear;
    public bool pushBackEnemies;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        enemiesNear = 0;
        pushBackEnemies = false;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesNear >= 5){
            timer += Time.deltaTime;
            if (timer >= 1f){
                StartCoroutine(PushEnemiesBackTimer());
                timer = 0f;
            }
        }
        else{
            timer = 0f;
        }
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Enemy"){
            enemiesNear++;
        }
    }

    void OnTriggerExit(Collider col){
        if (col.gameObject.tag == "Enemy"){
            enemiesNear--;
        }
    }

    IEnumerator PushEnemiesBackTimer()
    {
        pushBackEnemies = true;
        yield return new WaitForSeconds(1f);
        pushBackEnemies = false;
    } 
}
