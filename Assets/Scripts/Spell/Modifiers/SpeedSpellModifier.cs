using System;
using UnityEngine;

class SpeedSpellModifier : SpellModifier
{
    
    public override void ModifySpell(SpellBase spell)
    {
        _cooldownMultiplier = 1 / 1.5f;
        spell._speed += 1f;
        spell._speed *= 2;
        spell._cooldown *= _cooldownMultiplier;
    }
    
    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        Action oldBehavior = action.behaviour;
        Action spell = () =>
        {
            oldBehavior.Invoke();
            var obj = action._objectForSpell.transform;
            EffectManager.PlayEffectAtPosition("speedTrail", obj.position, obj.lossyScale).transform.SetParent(obj);
        };
        action.behaviour = spell;
        
        return action; // No change
    }
}