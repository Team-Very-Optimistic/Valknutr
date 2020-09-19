using System;
using UnityEngine;

class ShieldSpell : SpellBaseType
{
    private Transform player;
    private float offsetIncrement;
    public override void Init()
    {
        _cooldown = 5f;
        _speed = 50f;
        offsetIncrement = 45f;
        player = GameManager.Instance._player.transform;
        _objectForSpell = SpellManager.Instance.shieldObject;
        animationType = CastAnimation.Shield;
    }
    
    public override void SpellBehaviour(Spell spell)
    {
        for (int i = 0; i < _iterations; i++)
        {
            var p = GameObject.Instantiate(_objectForSpell, player.position + Vector3.up + player.forward * 0.7f, player.localRotation);
            float rotateBy = (float) Math.Ceiling(i / 2.0) * (i % 2 == 0 ? -1 : 1) * offsetIncrement;
            rotateBy += _posDiff.x * 90 + _posDiff.z * 90;
            p.transform.RotateAround(player.position,Vector3.up, rotateBy);
            p.transform.SetParent(player);
            p.AddComponent<Shield>().SetSpeed(_speed);
            _objectForSpell = p;
        }
    }
}