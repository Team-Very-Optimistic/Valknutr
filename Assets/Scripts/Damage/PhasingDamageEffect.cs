using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PhasingDamageEffect : DamageEffect
{
    private float timeLockDuration;

    public PhasingDamageEffect SetDuration(float ls)
    {
        this.timeLockDuration = ls;
        return this;
    }

    IEnumerator LockUnlock(NavMeshAgent agent)
    {
        // agent.enabled = false;
        var speed = agent.speed;
        agent.speed = 0;
        yield return new WaitForSeconds(timeLockDuration);
        if(agent)
            agent.speed = speed;
    }
    public override void CastDamageEffect(Collider other, float damage)
    {
        var enemy = other.GetComponent<Enemy>();
        if (!enemy) return;
        
        var ai = other.GetComponent<NavMeshAgent>();
        EffectManager.PlayEffectAtPosition("RainbowEffect", other.transform.position);
        AudioManager.PlaySoundAtPosition("lightBuff",  other.transform.position);
        GameManager.Instance.StartCoroutine(LockUnlock(ai));

    }
    
}