using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Gun Damage", menuName = "Gun Related/Gun Damage", order = 0)]
public class GunDamage : ScriptableObject
{
    public float baseDamage;
    public float critMultiplier;

    public float CalculateDamage(string tag)
    {
        return tag == "CritHitbox" ? baseDamage * critMultiplier : baseDamage;
    }
}
