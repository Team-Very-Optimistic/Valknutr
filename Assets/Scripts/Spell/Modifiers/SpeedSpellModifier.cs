using System;
using UnityEngine;

class SpeedSpellModifier : SpellModifier
{
    public float speedMultiplier = 2f;
    public override void ModifySpell(SpellBase spell)
    {
        spell.speed += 1f;
        spell.speed *= speedMultiplier;
        spell.cooldown *= _cooldownMultiplier;
    }

    public override SpellContext ModifyBehaviour(SpellContext ctx)
    {
        var oldBehavior = ctx.action;
        ctx.action = ctx2 =>
        {
            oldBehavior.Invoke(ctx2);
            var obj = ctx2.objectForSpell.transform;
            EffectManager.PlayEffectAtPosition("speedTrail", obj.position, obj.lossyScale).transform.SetParent(obj);
        };

        ctx.speed += 1f;
        ctx.speed *= speedMultiplier;
        ctx.cooldown *= _cooldownMultiplier;
        return ctx;
    }

    public override void UseValue()
    {
        speedMultiplier *= value;
    }

    public override Tooltip GetTooltip(SpellContext ctx)
    {
        return new Tooltip("Haste" + DefaultModTitle(ctx), $"Increases speed of affected entities by {speedMultiplier}% and {DefaultModBody(ctx)}");
    }
}