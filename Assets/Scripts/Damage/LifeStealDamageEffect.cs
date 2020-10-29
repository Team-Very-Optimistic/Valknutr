using System.Collections;
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
public class WeaknessDamageEffect : DamageEffect
{
    private float weaknessRatio;
    private float weaknessDuration;
    
    public WeaknessDamageEffect SetWeaknessDamageEffect(float ratio, float time)
    {
        weaknessDuration = time;
        this.weaknessRatio = ratio;
        return this;
    }

    public override void CastDamageEffect(Collider other, float damage)
    {
        var dmg = other.GetComponent<Damage>();
        if (dmg)
        {
            dmg.SetDamage(dmg.GetDamage() * weaknessRatio);
            GameManager.Instance.StartCoroutine(SetOriginal(dmg));
        }
    }

    IEnumerator SetOriginal(Damage dmg)
    {
        yield return new WaitForSeconds(weaknessDuration);
        if(dmg)
            dmg.SetDamage(dmg.GetDamage() / weaknessRatio);
    }
}

