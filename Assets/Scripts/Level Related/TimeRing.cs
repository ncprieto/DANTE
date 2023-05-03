using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRing : MonoBehaviour
{

    public float timeRingTimeAdded;

    private UI_Script UI;
    
    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("Canvas").GetComponent<UI_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Player"){
            UI.AddTime(timeRingTimeAdded);
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
