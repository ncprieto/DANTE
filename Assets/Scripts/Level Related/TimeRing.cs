using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRing : MonoBehaviour
{

    public float timeRingTimeAdded;
    public int waypointSystemIndex;

    private UI_Script UI;
    private WaypointSystem ringWaypoint;
    
    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("Canvas").GetComponent<UI_Script>();
        ringWaypoint = Camera.main.GetComponents<WaypointSystem>()[waypointSystemIndex];
        ringWaypoint.target = this.transform;
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Player"){
            UI.AddTime(timeRingTimeAdded);
            ringWaypoint.target = null;
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
