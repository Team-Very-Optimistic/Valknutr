using System;
using UnityEngine;

[CreateAssetMenu]
public class PoisonModifier : SpellModifier
{
    private float poisonRatio = 0.1f;
    private int poisonTicks = 3;
    private float duration = 3f;

    private PoisonDamageEffect _poisonDamageEffect;

    public override void UseValue()
    {
        poisonRatio *= value;
        poisonTicks = Mathf.RoundToInt(poisonTicks * value);
        //Use value was called before the copy which made the object not copy over.
    }

    public override void ModifySpell(SpellBase spell)
    {
        if (_poisonDamageEffect == null)
        {
            _poisonDamageEffect = new PoisonDamageEffect();
            _poisonDamageEffect.SetPoison(poisonRatio, poisonTicks, duration);
        }
    }

    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        Action oldBehavior = action._behaviour;
        Action spell = () =>
        {
            oldBehavior.Invoke();
            var dmg = action._objectForSpell.GetComponentElseAddIt<Damage>();
            if (dmg)
            {
                dmg.AddDamageEffect(_poisonDamageEffect);
            }
        };
        action._behaviour = spell;
        return action;
    }
    

    public override Tooltip GetTooltip()
    {
        return new Tooltip("Venomous Sting" + DefaultModTitle(), $"All damage caused by the spell will poison for {poisonTicks} ticks, by a proportion of {poisonRatio:P}" +
                                                                 $"the original damage. Last for {duration}s. {DefaultModBody()}");
    }
}