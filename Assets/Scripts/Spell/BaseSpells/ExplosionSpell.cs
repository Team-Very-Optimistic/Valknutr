﻿using System;
using UnityEngine;

[CreateAssetMenu]
public class ExplosionSpell : SpellBaseType
{
    public float radius = 5.0F;
    public float power = 10.0F;
    private Transform player;
    private Vector3 offset;
    public float _damage;

    public override void Init()
    {
        _damage = 5f;
        _speed = 3f;
        _cooldown = 8f;
        player = GameManager.Instance._player.transform;
        _objectForSpell = SpellManager.Instance.explosionObject;
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