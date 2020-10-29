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
        if(other.CompareTag("Enemy"))
            GameManager.Instance.AffectPlayerCurrHealth(damage * lifeStealRatio);
    }
    
}