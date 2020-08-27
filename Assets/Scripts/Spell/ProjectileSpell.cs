using System;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
class ProjectileSpell : SpellBaseType
{
    public float speed = 25f;
    public override void SpellBehaviour(Spell spell)
    {
        var p = Spell.Instantiate(_objectForSpell, spell.gameObject.transform.position, Quaternion.Euler(_posDiff));
        p.AddComponent<Projectile>().Launch(spell.gameObject.transform.forward, speed);
    }
}