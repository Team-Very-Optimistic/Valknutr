using System;
using UnityEngine;

public class ExplosiveBehaviour : SpellBehavior
{
    public float radius = 7.0F;
    public float power = 10.0F;
    private Transform player;
    private Vector3 offset;

    public override void Init()
    {
        _damage = 10f;
        _speed = 3f;
        _cooldown = 8f;
        offset = Vector3.up;
        player = GameManager.Instance._player.transform;
        _objectForSpell = SpellManager.Instance.explosionObject;
        animationType = CastAnimation.Bomb;
    }

    public override void SpellBehaviour(Spell spell)
    {
        var p = Instantiate(_objectForSpell, player.position + offset + player.forward * 0.7f,
            Quaternion.Euler(_posDiff));

        Explosive explosive = p.GetComponent<Explosive>();
        explosive.radius = radius;
        explosive.speed = _speed;
        explosive._damage = _damage;
        explosive.power = power;
        explosive.Launch(_posDiff + offset , _speed);
        _objectForSpell = p;
    }
}