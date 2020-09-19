using System;
using UnityEngine;

[CreateAssetMenu]
public class ExplosionBehavior : SpellBehavior
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
        player = GameManager.Instance._player.transform;
        _objectForSpell = SpellManager.Instance.explosionObject;
        animationType = CastAnimation.Bomb;
    }

    public override void SpellBehaviour(Spell spell)
    {
        var p = Instantiate(_objectForSpell, player.position + Vector3.up + player.forward * 0.7f,
            Quaternion.Euler(_posDiff));

        Explosive explosive = p.GetComponent<Explosive>();
        explosive.radius = radius;
        explosive.speed = _speed;
        explosive._damage = _damage;
        explosive.power = power;
        explosive.Launch(_posDiff, _speed);
        _objectForSpell = p;
    }
}