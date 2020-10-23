using System;

class FireSpellModifier : SpellModifier
{
    public float fireMultiplier = 1.2f;
    public float dmg = 1f;
    public override void ModifySpell(SpellBase spell)
    {
        //base.ModifySpell(spell);
        spell._speed *= fireMultiplier;
        spell._cooldown *= _cooldownMultiplier;
    }
    
    public override SpellBase ModifyBehaviour(SpellBase action)
    {
        //important to make sure it doesnt cast a recursive method
        Action oldBehavior = action._behaviour;
        Action spell = () =>
        {
            oldBehavior.Invoke();
            Fire fire = Fire.SpawnFire(action._objectForSpell);
            fire.Set(action);
        };
        action._behaviour = spell;
        return action;
    }

    public override void UseValue()
    {
        fireMultiplier *= value;
        dmg *= value;
    }

    public override Tooltip GetTooltip()
    {
        return new Tooltip("Fire" + DefaultModTitle(), $"Affected entities will be set on fire which afflicts {dmg:F} " +
                                              $"damage to other entities in contact. Speed is increased by {fireMultiplier:F} " +
                                              $"times." + DefaultModBody());
    }
}