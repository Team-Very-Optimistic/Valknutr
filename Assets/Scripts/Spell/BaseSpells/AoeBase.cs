using UnityEngine;

[CreateAssetMenu]
public class AoeBase : SpellBase
{
    [SerializeField]
    private float _aoeInterval = 0.5f;

    public float _duration = 4f;
    
    protected override void SetValues()
    {
        _objectForSpell = _player.gameObject;
        _duration = 3.75f;

    }
    protected override void AfterReset()
    {
        _aoeInterval /= _speed / properties._speed;
        _duration /= _speed / properties._speed;
    }

    public override void SpellBehaviour(Spell spell)
    {
        var aoe = AoeBlast.SpawnBlast(_objectForSpell);
        aoe.Set(this, _aoeInterval, _duration);
        _objectForSpell = aoe._aoeEffect;
    }
    
    public override Tooltip GetTooltip()
    {
        //SetValues();
        return new Tooltip($"Sphere blast {DefaultBaseTitle()}", $"Channels for {_duration:F}, dealing {_damage:F} damage every {_aoeInterval:F}s around you. {DefaultBaseBody()}");
    }
}