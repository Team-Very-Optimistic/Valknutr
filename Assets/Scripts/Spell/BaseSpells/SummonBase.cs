using System;
using System.Collections;
using UnityEngine;

class SummonBase : SpellBase
{
    public float _duration;
    
    protected override void SetValues()
    {
        _cooldown = 30f;
        _duration = 30f;
        _offset = _player.forward * 1.5f;
    }
    public override void SpellBehaviour(Spell spell)
    {
        _objectForSpell = Instantiate(_objectForSpell, _player.position + _offset, Quaternion.identity);
        _objectForSpell.GetComponent<Summon>().Set(_duration, _speed, _damage, _scale);
    }
    public override Tooltip GetTooltip(SpellContext ctx)
    {
        if (!ctx.useCtx) ctx = GetContext();
        return new Tooltip($"Summon {DefaultBaseTitle(ctx)}", $"Spawns a familiar that lasts {_duration}s. \n{DefaultBaseBody(ctx)}");
    }
}