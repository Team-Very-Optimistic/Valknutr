using System;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
class ProjectileBase : SpellBase
{
    private float offsetIncrement;
    
    protected override void SetValues()
    {
        _damage = 1;
        _speed = 25f;
        _cooldown = 0.09f;
        _offset = new Vector3(0f, 0f, 0f);
        offsetIncrement = 7f;
        _objectForSpell = SpellManager.Instance.projectileObject;
        animationType = CastAnimation.Projectile;
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
        _direction.y = 0;
        for (int i = 0; i < _iterations; i++)
        {
            var p = Instantiate(_objectForSpell, _player.position + _offset, Quaternion.Euler(_direction));
            double rotateBy = (float) (Math.Ceiling(i / 2.0) * (i % 2 == 0 ? -1 : 1) * offsetIncrement * Math.PI / 180);
            Vector3 newDirection = new Vector3((float) (_direction.x * Math.Cos(rotateBy) - _direction.z * Math.Sin(rotateBy)), 
                _direction.y, (float) (_direction.x * Math.Sin(rotateBy) + _direction.z * Math.Cos(rotateBy)));
            
            p.GetComponent<Projectile>().Launch(newDirection, _speed);
            
            ScreenShakeManager.Instance.ScreenShake(0.1f, 0.1f * _scale);
            p.GetComponent<Damage>().SetDamage(_damage);
            _objectForSpell = p;
        }
        
    }
}