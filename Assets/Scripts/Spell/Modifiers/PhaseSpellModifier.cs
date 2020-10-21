using System;
using UnityEngine;

class PhaseSpellModifier : SpellModifier
{
    public int phaseAmount = 3;

    public override SpellContext ModifyBehaviour(SpellContext ctx)
    {
        var oldBehavior = ctx.action;
        ctx.action = ctx2 =>
        {
            oldBehavior.Invoke(ctx2);
            var existingPhase = ctx2.objectForSpell.GetComponent<Phasing>();
            if(existingPhase == null)
                ctx2.objectForSpell.AddComponent<Phasing>()._damage = ctx2.damage;
            else
            {
                existingPhase.AddPhaseAmount(phaseAmount);
            }
        };
        
        ctx.cooldown *= _cooldownMultiplier;
        return ctx;
    }

    public override void UseValue()
    {
        phaseAmount = Mathf.RoundToInt(phaseAmount * value);
    }

    public override Tooltip GetTooltip(SpellContext ctx)
    {
        return new Tooltip("Phase" + DefaultModTitle(ctx), $"Causes affected entities to pass through solid objects up to {phaseAmount} times." + DefaultModBody(ctx));
    }
}