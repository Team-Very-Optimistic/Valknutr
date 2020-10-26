using UnityEngine;

public class LifeStealDamageEffect : DamageEffect
{
    private float lifeStealRatio;
    
    public LifeStealDamageEffect SetLifeSteal(float ls)
    {
        this.lifeStealRatio = ls;
        return this;
    }

    public override void CastDamageEffect(Collider other, float damage)
    {
        GameManager.Instance.AffectPlayerCurrHealth(damage * lifeStealRatio);
    }
}
public class WeaknessDamageEffect : DamageEffect
{
    private float weaknessRatio;
    
    public WeaknessDamageEffect SetWeaknessDamageEffect(float ratio)
    {
        this.weaknessRatio = ratio;
        return this;
    }

    public override void CastDamageEffect(Collider other, float damage)
    {
        var dmg = other.GetComponent<Damage>();
        if (dmg)
        {
            dmg.SetDamage(dmg.GetDamage() * weaknessRatio);
        }
    }
}

