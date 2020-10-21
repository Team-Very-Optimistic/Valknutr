using System;
using System.Collections;
using UnityEngine;

class GrowSpellModifier : SpellModifier
{
    public float sizeChange = 1.5f;
    public float cap = 1f;

    public override SpellContext ModifyBehaviour(SpellContext ctx)
    {
        var oldBehavior = ctx.action;
        ctx.action = ctx2 =>
        {
            oldBehavior.Invoke(ctx2);
            var transformLocalScale = ctx2.objectForSpell.transform.localScale;
            if (transformLocalScale.x > cap) return;
            ctx2.objectForSpell.transform.localScale = transformLocalScale * sizeChange;
            GameManager.Instance.StartCoroutine(MakeSmall(ctx2.objectForSpell, transformLocalScale));
        };
        
        ctx.scale *= sizeChange;
        ctx.damage *= sizeChange;
        ctx.cooldown *= _cooldownMultiplier;
        return ctx;
    }

    public override void UseValue()
    {
        sizeChange *= value;
        cap *= value;
    }

    // public override void ModifySpell(SpellBase spell)
    // {
    //     var varSize = Math.Log(spell.cooldown, 1000) + 1.7f;
    //     sizeChange = (float) Math.Max(1.01f, varSize);;
    //     base.ModifySpell(spell);
    //     spell.scale *= sizeChange;
    //     cap = spell.scale * 2;
    // }

    IEnumerator MakeSmall(GameObject obj, Vector3 size)
    {
        if (obj == null)
        {
            
        }
        else if (obj.transform.localScale.magnitude > size.magnitude)
        {
            obj.transform.localScale *= 0.99f;
            yield return new WaitForSeconds(0.15f);
            GameManager.Instance.StartCoroutine(MakeSmall(obj, size));
        }
    }

    public override Tooltip GetTooltip(SpellContext ctx)
    {
        return new Tooltip("Size+" + DefaultModTitle(ctx), 
            $"Increases size of affected entities by {sizeChange} times. Capped at {cap} times the original size." + DefaultModBody(ctx));
    }
}