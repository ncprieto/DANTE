using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiStuck : MonoBehaviour
{

    public int enemiesNear;
    public bool pushBackEnemies;

    private float timer;
    private UnityEngine.Object divineCyl;

    // Start is called before the first frame update
    void Start()
    {
        enemiesNear = 0;
        pushBackEnemies = false;
        timer = 0f;
        divineCyl = Resources.Load("Prefabs/DivineCylinder");
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesNear >= 2){
            timer += Time.deltaTime;
            if (timer >= 1f){
                StartCoroutine(PushEnemiesBackTimer());
                Instantiate(divineCyl, new Vector3(transform.position.x, transform.position.y + 6, transform.position.z), Quaternion.identity, transform);
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
            if (enemiesNear > 0){
                enemiesNear--;
            }
        }
    }

    IEnumerator PushEnemiesBackTimer()
    {
        pushBackEnemies = true;
        yield return new WaitForSeconds(.5f);
        pushBackEnemies = false;
    } 
}
