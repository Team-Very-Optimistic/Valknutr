using System;
using System.Collections;
using UnityEngine;

class SummonBase : SpellBase
{
    [SerializeField]
    private float _duration;
    
    protected override void SetValues()
    {
        // cooldown = 30f;
        _duration = 30f;
        _offset = _player.forward * 1.5f;
    }
    public override void SpellBehaviour(Spell spell)
    {
        _objectForSpell = Instantiate(_objectForSpell, _player.position + _offset, Quaternion.identity);
        _objectForSpell.GetComponent<Summon>().Set(_duration, _speed, _damage, _scale);
    }
    public override Tooltip GetTooltip()
    {
        return new Tooltip($"Summon {DefaultBaseTitle()}", $"Spawns a familiar that lasts {_duration}s. Follows your guided direction. \n{DefaultBaseBody()}");
    }
}