using UnityEngine;

public class DamageIncreaseModifier : SpellModifier
{
    public float damageMultiplier = 2;

    public override void ModifySpell(SpellBase spell)
    {
        base.ModifySpell(spell);
        spell._damage *= damageMultiplier;
    }

    public override void UseValue()
    {
        damageMultiplier = damageMultiplier * value;
    }
    
    public override Tooltip GetTooltip()
    {
        return new Tooltip("Damage Amplification" + DefaultModTitle(), $"Increases damage of spell by {damageMultiplier:F} times. {DefaultModBody()}");
    }
}