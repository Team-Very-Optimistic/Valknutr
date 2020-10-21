using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

class SplitSpellModifier : SpellModifier
{
    public int iterations = 2;
    public float damageReduction = 0.8f;
    public float randomMax = 0.2f;

    public override SpellContext ModifyBehaviour(SpellContext ctx)
    {
        var oldBehavior = ctx.action;
        ctx.action = ctx2 =>
        {
            Vector3 originalPosDiff = ctx2.direction;
            for (int i = iterations - 1; i >= 0; i--)
            {
                ctx2.direction += new Vector3(Random.Range(-randomMax, randomMax), 0,
                    Random.Range(-randomMax, randomMax));
                ctx2.offset += new Vector3(Random.Range(-randomMax, randomMax), 0,
                    Random.Range(-randomMax, randomMax));
                ctx2.direction.Normalize();
                oldBehavior.Invoke(ctx2);
            }
            ctx2.direction = originalPosDiff; //reset
        };

        ctx.damage *= damageReduction;
        ctx.cooldown *= _cooldownMultiplier;
        // ctx.iterations += iterations;
        return ctx;
    }

    public override void UseValue()
    {
        iterations = Mathf.RoundToInt(iterations * value);
        damageReduction = value / (iterations / 2);
    }

    IEnumerator DelayInvoke(Action invoke, float delay)
    {
        yield return new WaitForSeconds(delay);
        invoke.Invoke();
    }

    public override Tooltip GetTooltip()
    {
        return new Tooltip("Split" + DefaultModTitle(),
            $"Repeats the spell effects {iterations} times, but each spell effect is {damageReduction}% weaker. {DefaultModBody()}");
    }
}