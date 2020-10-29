using UnityEngine;

public class PoisonDamageEffect : DamageEffect
{
    private float poisonRatio;
    private int poisonTicks;
    private float duration;
    
    public PoisonDamageEffect SetPoison(float ls, int ticks, float dura)
    {
        this.poisonRatio = ls;
        poisonTicks = ticks;
        duration = dura;
        return this;
    }

    public override void CastDamageEffect(Collider other, float damage)
    {
        other.GetComponent<HealthScript>().ApplyDamageOverTime(damage, poisonTicks, duration, Color.green);
    }
    
}