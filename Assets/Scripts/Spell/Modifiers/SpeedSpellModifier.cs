using System;
using UnityEngine;

class SpeedSpellModifier : SpellModifier
{
    [SerializeField]
    private float speedMultiplier = 2f;
    public override void ModifySpell(SpellBase spell)
    {
        spell._speed += 1f;
        spell._speed *= speedMultiplier;
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

    public override void UseQuality()
    {
        speedMultiplier *= quality;
    }
}