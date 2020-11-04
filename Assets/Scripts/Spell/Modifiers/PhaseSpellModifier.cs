using System;
using UnityEngine;

class PhaseSpellModifier : SpellModifier
{
    public int phaseAmount = 3;
    public float lockDuration = 1f;
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
                phase.Set(action, lockDuration, phaseAmount);
            }
        };
        action._behaviour = spell;
        return action;
    }

    public override void UseValue()
    {
        phaseAmount = Mathf.RoundToInt(phaseAmount * value);
        lockDuration *= value;
    }

    public override Tooltip GetTooltip()
    {
        return new Tooltip("Phase" + DefaultModTitle(), $"Causes spell object to pass through solid objects up to {phaseAmount} times. " +
                                                        $"Any enemies that pass through will be locked in place for {lockDuration:F} seconds." + DefaultModBody());
    }
}