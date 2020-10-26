using System;
using UnityEngine;

class PhaseSpellModifier : SpellModifier
{
    public int phaseAmount = 3;
    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action._behaviour;
        
        Action spell = () =>
        {
            oldBehavior.Invoke();
            if (action._objectForSpell)
            {
                var phase = action._objectForSpell.GetComponentElseAddIt<Phasing>();
                phase.Set(action);
            }
        };
        action._behaviour = spell;
        return action;
    }

    public override void UseValue()
    {
        phaseAmount = Mathf.RoundToInt(phaseAmount * value);
    }

    public override Tooltip GetTooltip()
    {
        return new Tooltip("Phase" + DefaultModTitle(), $"Causes affected entities to pass through solid objects up to {phaseAmount} times." + DefaultModBody());
    }
}