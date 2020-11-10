using System;
using UnityEngine;

public class ExplosiveBase : SpellBase
{
    // todo: scale with dmg and scale
    public float radius = 5.0F;
    public float power = 10.0F;

    protected override void SetValues()
    {
        power = 50.0F;
    }

    protected override void AfterReset()
    {
        power *= radius;
        _offset += _player.forward * 1.3f;
        _direction = _direction * 2 + _offset;
        radius = 3 * _scale;
    }
    
    /// <summary>
    /// todo: use the following properties:
    /// _direction: yes
    /// _objectForSpell: yes
    /// _speed: yes
    /// _damage: yes
    /// _offset: yes
    /// _objectsCollided: 
    /// _trigger: yes
    /// </summary>
    public override void SpellBehaviour(Spell spell)
    {
        if (!_objectForSpell)
        {
            _objectForSpell = properties._objectForSpell;
        }
        
        var p = Instantiate(_objectForSpell, _player.position + _offset,
            Quaternion.Euler(_direction));
        
        Explosive explosive = p.GetComponent<Explosive>();

        explosive.radius = radius;
        explosive._damage = _damage;
        explosive.power = power;
        explosive.Launch(_direction, _speed);
        _objectForSpell = p;
    }
    
    public override Tooltip GetTooltip()
    {
        return new Tooltip($"Bomb {DefaultBaseTitle()}", $"Creates an explosive that detonates on contact, dealing {_damage:F} to entities in a radius of {radius:F} with " +
                                                         $"an explosive power of {power:F}. {DefaultBaseBody()}", $"Level {level}");
    }
}