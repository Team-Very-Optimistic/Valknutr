using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
        if (!healthScript) return;
        healthScript.StartCoroutine(healthScript.ApplyDamageOverTime(poisonRatio *damage, poisonTicks, duration, new Color(0.2f, 1f, 0.2f)));
        EffectManager.PlayEffectAtPosition("PoisonEffect", other.transform.position, other.transform.localScale/10).transform.SetParent(other.transform);
        AudioManager.PlaySoundAtPosition("poison",  other.transform.position);
    }
    
}

public class PhasingDamageEffect : DamageEffect
{
    private float timeLockDuration;

    public PhasingDamageEffect SetDuration(float ls)
    {
        this.timeLockDuration = ls;
        return this;
    }

    IEnumerator Unlock(NavMeshAgent agent)
    {
        yield return new WaitForSeconds(timeLockDuration);
        if(agent)
            agent.enabled = true;
    }
    public override void CastDamageEffect(Collider other, float damage)
    {
        var enemy = other.GetComponent<Enemy>();
        if (!enemy) return;
        
        var ai = other.GetComponent<NavMeshAgent>();
        ai.enabled = false;
        EffectManager.PlayEffectAtPosition("RainbowEffect", other.transform.position);
        AudioManager.PlaySoundAtPosition("lightBuff",  other.transform.position);
        GameManager.Instance.StartCoroutine(Unlock(ai));

    }
    
}