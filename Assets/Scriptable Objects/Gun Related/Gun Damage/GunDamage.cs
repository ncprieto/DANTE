using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Gun Damage", menuName = "Gun Related/Gun Damage", order = 0)]
public class GunDamage : ScriptableObject
{
    public AnimationCurve falloffCurve;
    public float critMultiplier;
    public AnimationCurve bHopMultiplier;

    public float CalculateDamage(float distance, int bHopCount, string hitboxTag)
    {
        float damage = 0f;
        ApplyDamageFalloff(ref  damage, distance);
        ApplyBHopMultiplier(ref damage, bHopCount);
        ApplyCritMultiplier(ref damage, hitboxTag);
        return damage;
    }

    public float GetBHopMultiplier(int bHopCount)
    {
        return bHopMultiplier.Evaluate(bHopCount);
    }

    private void ApplyDamageFalloff(ref float damage, float distance)
    {
        damage = falloffCurve.Evaluate(distance);
    }

    private void ApplyBHopMultiplier(ref float damage, int bHopCount)
    {
        if(bHopCount == 0) return;
        damage = damage * bHopMultiplier.Evaluate(bHopCount);
    }

    private void ApplyCritMultiplier(ref float damage, string hitbox)
    {
        damage *= hitbox == "CritHitbox" ? critMultiplier : 1;
    }
}
