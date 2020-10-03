using System;
using UnityEngine;

public class ExplosiveBase : SpellBase
{
    public float radius = 7.0F;
    public float power = 1000.0F;

    public override void Init()
    {
        _damage = 10f;
        _speed = 5f;
        _cooldown = 8f;
        _offset = Vector3.up  + _player.forward * 0.7f;
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
        radius *= _objectForSpell.transform.localScale.x;
        explosive.radius = radius;
        explosive.speed = _speed;
        explosive._damage = _damage;
        explosive.power = power * _damage / 10f + power * radius;
        explosive.Launch(_direction + _offset , _speed);
        _objectForSpell = p;
    }
}