﻿using System;
using System.Security.Cryptography;
using UnityEngine;
 using Random = UnityEngine.Random;

 [Serializable]
class ProjectileBase : SpellBase
{
    // private float offsetIncrement = 7f;
    
    // protected override void SetValues()
    // {
    //     offsetIncrement = 7f;
    // }
    
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
    public override void SpellBehaviour(SpellContext ctx)
    {
        ctx.direction.y = 0;

        var p = Instantiate(objectForSpell, _player.position + ctx.offset, Quaternion.Euler(ctx.direction));
        // double rotateBy = (float) (Math.Ceiling(i / 2.0) * (i % 2 == 0 ? -1 : 1) * offsetIncrement * Math.PI / 180);
        // var rotateBy = 0f;
        // Vector3 newDirection = new Vector3((float) (ctx.direction.x * Math.Cos(rotateBy) - ctx.direction.z * Math.Sin(rotateBy)), 
        //     ctx.direction.y, (float) (ctx.direction.x * Math.Sin(rotateBy) + ctx.direction.z * Math.Cos(rotateBy)));
        
        p.GetComponent<Projectile>().Launch(ctx.direction, ctx.speed, ctx.damage);
        
        SpellEffects(true, 0.1f, 0.1f);
        ctx.objectForSpell = p;
    }
    
    public override Tooltip GetTooltip(SpellContext ctx)
    {
        if (!ctx.useCtx) ctx = GetContext();
        return new Tooltip($"Projectile {DefaultBaseTitle(ctx)}", $"Sends forth a projectile that deals {ctx.damage:F1} to enemies that it collides into. {DefaultBaseBody(ctx)}");
    }
}