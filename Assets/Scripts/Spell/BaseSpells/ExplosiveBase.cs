using System;
using UnityEngine;

public class ExplosiveBase : SpellBase
{
    // todo: scale with dmg and scale
    public float radius = 5.0F;
    public float power = 100.0F;

    public override SpellContext GetContext()
    {
        var ctx = base.GetContext();
        ctx.offset = Vector3.up;
        return ctx;
    }

    // protected override void SetValues()
    // {
    //
    //     radius = 3.0F;
    //     power = 100.0F;
    //     offset = Vector3.up  + _player.forward * 1.3f;
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
        var p = Instantiate(objectForSpell, _player.position + ctx.offset,
            Quaternion.Euler(direction));
        
        Explosive explosive = p.GetComponent<Explosive>();
        
        explosive.radius = radius * ctx.scale;
        explosive._damage = ctx.damage;
        explosive.power = power * ctx.scale;
        explosive.Launch(ctx.direction * 2 + ctx.offset, ctx.speed);
        ctx.objectForSpell = p;
    }
    
    public override Tooltip GetTooltip(SpellContext ctx)
    {
        return new Tooltip($"Bomb {DefaultBaseTitle(ctx)}", $"Creates an explosive that detonates on contact, dealing {ctx.damage:F1} to entities in a radius of {radius * ctx.scale:F1}. {DefaultBaseBody(ctx)}");
    }
}