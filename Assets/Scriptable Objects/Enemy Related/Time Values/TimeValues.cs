using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Time Values", menuName = "Enemy Related/Time Values", order = 0)]
public class TimeValues : ScriptableObject
{
    public float baseTimeReward;
    public float critMultiplier;

    public float GetRewardValue(string hitbox)
    {
        return hitbox == "CritHitbox" ? baseTimeReward * critMultiplier : baseTimeReward;
    }
}
