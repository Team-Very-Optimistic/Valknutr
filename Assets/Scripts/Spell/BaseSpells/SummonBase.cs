using System;
using System.Collections;
using UnityEngine;

class SummonBase : SpellBase
{
    [SerializeField] private float _duration;

    protected override void SetValues()
    {
        // cooldown = 30f;
        _duration = 30f;
        _offset = _player.forward * 1.5f;
    }

    protected override void AfterReset()
    {
        _duration += _scale + _damage;
    }


    public override void SpellBehaviour(Spell spell)
    {
        _objectForSpell = Instantiate(_objectForSpell, _player.position + _offset, Quaternion.identity);
        _objectForSpell.GetComponentElseAddIt<Summon>().Set(this, _duration);
    }

    public override Tooltip GetTooltip()
    {
        return new Tooltip($"Summon {DefaultBaseTitle()}", $"Spawns a familiar that lasts {_duration:0.##}s. " +
                                                           $"Follows your guided direction. {(quality == QualityManager.Quality.Sanctified ? "Be wary, for this may be your last chance to get close to nexus." : "")}\n{DefaultBaseBody()}", $"Level {level}");
    }
}