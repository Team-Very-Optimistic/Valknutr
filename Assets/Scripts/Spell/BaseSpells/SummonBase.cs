using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu]
class SummonBase : SpellBase
{
    public float _duration;
    
    protected override void SetValues()
    {
        _cooldown = 30f;
        _duration = 30f;
    }
    public override void SpellBehaviour(Spell spell)
    {
        _objectForSpell = Instantiate(_objectForSpell, _player.position + _offset, Quaternion.identity);
        _objectForSpell.GetComponent<Summon>().Set(_duration, _speed, _damage, _scale);
    }

    public override Tooltip GetTooltip()
    {
        return default;
    }
}