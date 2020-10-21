using System;
using UnityEngine;

public class ExplosiveBase : SpellBase
{
    // todo: scale with dmg and scale
    public float radius = 5.0F;
    public float power = 100.0F;

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
        explosive.Launch(direction * 2 + offset, speed);
        ctx.objectForSpell = p;
    }
    
    public override Tooltip GetTooltip()
    {
        return new Tooltip($"Bomb {DefaultBaseTitle()}", $"Creates an explosive that detonates on contact, dealing {damage} to entities in a radius of {radius}. {DefaultBaseBody()}");
    }
}