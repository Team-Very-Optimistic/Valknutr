using System;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
class ProjectileSpell : SpellBaseType
{
    private Transform player;
    private Vector3 offset;
    private float offsetIncrement;

    
    public override void Init()
    {
        _damage = 1;
        _speed = 25f;
        _cooldown = 0.09f;
        offset = new Vector3(0f, 0.85f, 0f);
        offsetIncrement = 7f;
        _objectForSpell = SpellManager.Instance.projectileObject;
        player = GameManager.Instance._player.transform;
        animationType = CastAnimation.Projectile;
    }
    public override void SpellBehaviour(Spell spell)
    {

        _posDiff.y = 0;
        for (int i = 0; i < _iterations; i++)
        {
            var p = GameObject.Instantiate(_objectForSpell, player.position + offset, Quaternion.Euler(_posDiff));
            double rotateBy = (float) (Math.Ceiling(i / 2.0) * (i % 2 == 0 ? -1 : 1) * offsetIncrement * Math.PI / 180);
            Vector3 newDirection = new Vector3((float) (_posDiff.x * Math.Cos(rotateBy) - _posDiff.z * Math.Sin(rotateBy)), 
                _posDiff.y, (float) (_posDiff.x * Math.Sin(rotateBy) + _posDiff.z * Math.Cos(rotateBy)));
            
            p.GetComponent<Projectile>().Launch(newDirection, _speed);
            p.GetComponent<Damage>().SetDamage(_damage);
            _objectForSpell = p;
        }
        
    }
}