using System;
using System.Collections;
using UnityEngine;

class SummonBase : SpellBase
{
    // protected override void SetValues()
    // {
    //     cooldown = 30f;
    //     duration = 30f;
    //     offset = _player.forward * 1.5f;
    // }
    public override void SpellBehaviour(SpellContext ctx)
    {
        ctx.objectForSpell = Instantiate(objectForSpell, _player.position + ctx.offset, Quaternion.identity);
        ctx.objectForSpell.GetComponent<Summon>().Set(ctx.duration, ctx.speed, ctx.damage, ctx.scale);
    }
    public override Tooltip GetTooltip(SpellContext ctx)
    {
        return new Tooltip($"Summon {DefaultBaseTitle(ctx)}", $"Spawns a familiar that lasts {_duration}s. \n{DefaultBaseBody(ctx)}");
    }
}