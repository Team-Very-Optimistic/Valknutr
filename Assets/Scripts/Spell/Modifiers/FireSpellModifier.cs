﻿using System;

class FireSpellModifier : SpellModifier
{
    private float fireMultiplier = 1.2f;
    private float dmg = 1f;
    public override void ModifySpell(SpellBase spell)
    {
        //base.ModifySpell(spell);
        spell._speed *= fireMultiplier;
        spell._cooldown *= _cooldownMultiplier;
    }
    
    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action.behaviour;
        Action spell = () =>
        {
            oldBehavior.Invoke();
            Fire fire = action._objectForSpell.AddComponent<Fire>();
            fire.damage = dmg;
            fire.SetInitializer();
        };
        action.behaviour = spell;
        return action;
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