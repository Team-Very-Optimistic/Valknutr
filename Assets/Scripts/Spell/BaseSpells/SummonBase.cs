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
        _offset = _player.forward * 1.5f;
    }
    public override void SpellBehaviour(Spell spell)
    {
        _objectForSpell = Instantiate(_objectForSpell, _player.position + _offset, Quaternion.identity);
        _objectForSpell.GetComponent<Summon>().Set(_duration, _speed, _damage, _scale);
    }
}