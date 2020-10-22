using System;
using UnityEngine;

public struct SpellContext
{
    public Action<SpellContext> action;
    public SpellBase spellBase;
    public SpellModifier[] spellModifiers;
    public GameObject objectForSpell; //spell cast in reference to this object
    public float damage;
    public float speed;
    public float scale;
    public float cooldown;
    public float duration;
    public int iterations;
    public Vector3 direction; //The vector direction
    public Vector3 offset; //The vector offset for any behaviour
    public bool useCtx;

    public SpellContext(Action<SpellContext> action, SpellBase spellBase, SpellModifier[] spellModifiers, GameObject objectForSpell, float damage, float speed, float scale, float cooldown, Vector3 direction, Vector3 offset, float duration, int iterations)
    {
        this.spellBase = spellBase;
        this.objectForSpell = objectForSpell;
        this.damage = damage;
        this.speed = speed;
        this.scale = scale;
        this.cooldown = cooldown;
        this.direction = direction;
        this.offset = offset;
        this.duration = duration;
        this.spellModifiers = spellModifiers;
        this.action = action;
        this.iterations = iterations;
        useCtx = true;
    }
}