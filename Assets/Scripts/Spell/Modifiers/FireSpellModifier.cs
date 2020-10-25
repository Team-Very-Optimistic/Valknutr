using System;

class FireSpellModifier : SpellModifier
{
    public float fireMultiplier = 1.2f;
    public float dmgRatio = 0.2f;
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
            fire.Set(action, dmgRatio);
        };
        action._behaviour = spell;
        return action;
    }

    public override void UseValue()
    {
        fireMultiplier *= value;
        dmgRatio *= value;
    }

    public override Tooltip GetTooltip()
    {
        return new Tooltip("Fire" + DefaultModTitle(), $"Affected entities will be set on fire which afflicts a percent ({dmgRatio:P}%) " +
                                              $"of the spell's damage to other entities in contact. Speed is increased by {fireMultiplier:F} " +
                                              $"times." + DefaultModBody());
    }
}