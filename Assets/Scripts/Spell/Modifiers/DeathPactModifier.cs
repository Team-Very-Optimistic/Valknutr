using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class DeathPactModifier : SpellModifier
{
    public float healthRatio = 0.1f;
    public float damageMultiplier = 3f;
    public float speedMultiplier = 1.3f;
    public float vulnerabilityPeriod = 3f;
    public float vulnerabilityAmp = 2f;
    
    public override void ModifySpell(SpellBase spell)
    {
        base.ModifySpell(spell);
        spell._damage *= damageMultiplier;
        spell._speed += 1f;
        spell._speed *= speedMultiplier;
        spell._cooldown *= _cooldownMultiplier;
        
    }

    public override void UseValue()
    {
        speedMultiplier *= value;
        damageMultiplier *= value;
        vulnerabilityPeriod /= value;
    }
    
    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        Action oldBehavior = action._behaviour;
        Action spell = () =>
        {
            GameManager.Instance.AffectPlayerCurrHealth(GameManager.Instance._playerHealth.maxHealth * healthRatio * -1);
            GameManager.Instance._playerHealth.AdditivelyAddDmgMultiplier(vulnerabilityAmp);
            GameManager.Instance.StartCoroutine(ResetWeakness(vulnerabilityPeriod));
            oldBehavior.Invoke();
        };
        action._behaviour = spell;
        return action;
    }

    private IEnumerator ResetWeakness(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance._playerHealth.AdditivelyAddDmgMultiplier(1/vulnerabilityPeriod);
    }

    public override Tooltip GetTooltip()
    {
        return new Tooltip("Death Pact" + DefaultModTitle(), $"Partake in a trade with Nexus, reduce spell cooldown by {_cooldownMultiplier:P1}, " +
                                                             $"increase damage by {damageMultiplier:P1}, increase speed by {speedMultiplier:P1}. However," +
                                                             $"everytime the spell is cast, lose {healthRatio:P1} of your health and gain " +
                                                             $"weakness, all damage against you increases by {vulnerabilityAmp:P1} for {vulnerabilityPeriod:0.##}s. {DefaultModBody()}");
    }
}