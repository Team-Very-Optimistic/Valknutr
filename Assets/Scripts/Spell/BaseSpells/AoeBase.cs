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
    }
    
    
    public override void SpellBehaviour(Spell spell)
    {
        _aoeInterval = 0.5f * _speed / properties._speed;
        _duration = 4f * _speed / properties._speed;
        var aoe = _objectForSpell.AddComponent<AoeBlast>();
        aoe.Set(_damage, _aoeInterval, _duration);
        _objectForSpell = aoe._aoeEffect;
    }
    
    public override Tooltip GetTooltip()
    {
        //SetValues();
        return new Tooltip($"Sphere blast {DefaultBaseTitle()}", $"Channels for {_duration:F}, dealing {_damage:F} damage every {_aoeInterval:F}s around you. {DefaultBaseBody()}");
    }
}