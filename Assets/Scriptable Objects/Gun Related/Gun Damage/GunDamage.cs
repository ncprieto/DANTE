using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Gun Damage", menuName = "Gun Related/Gun Damage", order = 0)]
public class GunDamage : ScriptableObject
{
    public float baseDamage;
    public float critMultiplier;
    public AnimationCurve damageBHopMultiplier;

    public float CalculateDamage(string tag, int bHopCount)
    {
        float damage = tag == "CritHitbox" ? baseDamage * critMultiplier : baseDamage;
        if(bHopCount != 0) damage = damage * damageBHopMultiplier.Evaluate(bHopCount);
        return damage;
    }
}
