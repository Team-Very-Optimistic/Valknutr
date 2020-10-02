using System;
using UnityEngine;

class PhaseSpellModifier : SpellModifier
{
    
    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action.behaviour;
        
        Action spell = () =>
        {
            oldBehavior.Invoke();
            action._objectForSpell.AddComponent<Phasing>()._damage = action._damage;
        };
        
        action.behaviour = spell;
        return action;
    }
}