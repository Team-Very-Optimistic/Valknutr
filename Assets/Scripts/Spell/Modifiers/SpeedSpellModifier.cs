using System;
using UnityEngine;

class SpeedSpellModifier : SpellModifier
{
    public float speedMultiplier = 2f;
    public override void ModifySpell(SpellBase spell)
    {
        spell._speed += 1f;
        spell._speed *= speedMultiplier;
        spell._cooldown *= _cooldownMultiplier;
    }
    
    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        Action oldBehavior = action._behaviour;
        Action spell = () =>
        {
            oldBehavior.Invoke();
            var obj = action._objectForSpell.transform;
            EffectManager.PlayEffectAtPosition("speedTrail", obj.position, obj.lossyScale).transform.SetParent(obj);
        };
        action._behaviour = spell;
        
        return action; // No change
    }

    public override void UseValue()
    {
        speedMultiplier *= value;
    }

    public override Tooltip GetTooltip()
    {
        return new Tooltip("Haste" + DefaultModTitle(), $"Increases speed of the spell by {speedMultiplier:F} times. {DefaultModBody()}");
    }
}