using System;
using UnityEngine;

[CreateAssetMenu]
public class LifeStealModifier : SpellModifier
{
    public float lifeStealRatio = 0.02f;
    LifeStealDamageEffect _lifeStealDamageEffect;

    public override void UseValue()
    {
        lifeStealRatio *= value;
        _lifeStealDamageEffect  = new LifeStealDamageEffect().SetLifeSteal(lifeStealRatio) ;
    }

    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        Action oldBehavior = action._behaviour;
        Action spell = () =>
        {
            oldBehavior.Invoke();
            var dmg = action._objectForSpell.GetComponent<Damage>();
            if (dmg)
            {
                dmg.AddDamageEffect(_lifeStealDamageEffect);
            }
        };
        action._behaviour = spell;
        return action;
    }

    public override Tooltip GetTooltip()
    {
        return new Tooltip("Blood Resurgence" + DefaultModTitle(), $"All damage caused by the spell will heal you by a proportion of {lifeStealRatio:P}. {DefaultModBody()}");
    }
}