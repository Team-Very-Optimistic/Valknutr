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
        var healthScript = other.GetComponent<HealthScript>();
        healthScript.StartCoroutine(healthScript.ApplyDamageOverTime(poisonRatio *damage, poisonTicks, duration, new Color(0.2f, 1f, 0.2f)));
    }
    
}