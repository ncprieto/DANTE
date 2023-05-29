using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRing : MonoBehaviour
{
    public float timeRingTimeAdded;
    public int waypointSystemIndex;
    public TimeSource source;
    private WaypointSystem ringWaypoint;
    
    // Start is called before the first frame update
    void Start()
    {
        ringWaypoint = Camera.main.GetComponents<WaypointSystem>()[waypointSystemIndex];
        ringWaypoint.target = this.transform;
        SetUpModifiers();
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Player"){
            source.ReceiveTimeFromSource(timeRingTimeAdded);
            ringWaypoint.target = null;
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }

    private void SetUpModifiers()
    {
        ApplyModifier("Time Per Ring", ref timeRingTimeAdded);
    }

    private void ApplyModifier(string modifierName, ref float value)
    {
        value *= PlayerPrefs.GetFloat(modifierName, 1);
    }

    private void ApplyModifier(string modifierName, ref int value)
    {
        value *= (int)PlayerPrefs.GetFloat(modifierName, 1);
    }
}
