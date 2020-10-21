using System;

class FireSpellModifier : SpellModifier
{
    public float fireMultiplier = 1.2f;
    public float dmg = 1f;

    // public override void ModifySpell(SpellBase spell)
    // {
    //     //base.ModifySpell(spell);
    //     spell.speed *= fireMultiplier;
    //     spell.cooldown *= _cooldownMultiplier;
    // }

    public override SpellContext ModifyBehaviour(SpellContext ctx)
    {
        var oldBehavior = ctx.action;
        ctx.action = ctx2 =>
        {
            oldBehavior.Invoke(ctx2);
            Fire fire = ctx2.objectForSpell.AddComponent<Fire>();
            fire.damage = dmg;
            fire.SetInitializer();
        };

        ctx.speed *= fireMultiplier;
        ctx.cooldown *= _cooldownMultiplier;
        return ctx;
    }

    public override void UseValue()
    {
        fireMultiplier *= value;
        dmg *= value;
    }

    public override Tooltip GetTooltip()
    {
        return new Tooltip("Fire" + DefaultModTitle(), $"Affected entities will be set on fire which afflicts {dmg} " +
                                                       $"damage to other entities in contact. Speed is increased by {fireMultiplier} " +
                                                       $"times." + DefaultModBody());
    }
}