using System;
using UnityEngine;

public class ExplosiveBase : SpellBase
{
    public float radius = 5.0F;
    public float power = 100.0F;

    public override void Init()
    {
        _damage = 10f;
        radius = 5.0F;
        power = 100.0F;
        _scale = 1;
        _speed = 5.5f;
        _cooldown = 5f;
        _offset = Vector3.up  + _player.forward * 1.5f;
        _objectForSpell = SpellManager.Instance.explosionObject;
        animationType = CastAnimation.Bomb;
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
        var p = Instantiate(_objectForSpell, _player.position + _offset,
            Quaternion.Euler(_direction));
        
        Explosive explosive = p.GetComponent<Explosive>();

        radius *= _scale;

        explosive.radius = radius;
        explosive._damage = _damage;
        explosive.power = power * _damage / 10f + power * radius;
        explosive.Launch(_direction * 2 + _offset, _speed);
        _objectForSpell = p;
    }
}