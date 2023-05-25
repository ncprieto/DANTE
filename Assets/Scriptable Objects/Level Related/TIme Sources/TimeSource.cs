using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Time Source", menuName = "Level Related/Time Source", order = 0)]
public class TimeSource : ScriptableObject
{
    public int tracker;
    public float timeGainedFromSource;
    private TimeUpdater timeKeeper;

    void Reset()
    {
        tracker = 0;
        timeGainedFromSource = 0;
    }

    public void Initialize(TimeUpdater time)
    {
        Reset();
        timeKeeper = time;
    }

    public void ReceiveTimeFromSource(float amount)
    {
        tracker ++;
        timeGainedFromSource += amount;
        if(timeKeeper != null) timeKeeper.ReceiveTime(amount);
    }
}
