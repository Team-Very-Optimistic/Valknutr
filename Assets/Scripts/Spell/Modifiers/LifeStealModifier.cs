using System;
using UnityEngine;

[CreateAssetMenu]
public class LifeStealModifier : SpellModifier
{
    public float lifeStealRatio = 0.02f;
    private LifeStealDamageEffect _lifeStealDamageEffect;

    public override void UseValue()
    {
        lifeStealRatio *= value;
        //Use value was called before the copy which made the object not copy over.
    }

    public override void ModifySpell(SpellBase spell)
    {
        if (_lifeStealDamageEffect == null)
        {
            _lifeStealDamageEffect = new LifeStealDamageEffect();
            _lifeStealDamageEffect.SetLifeSteal(lifeStealRatio);
        }
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