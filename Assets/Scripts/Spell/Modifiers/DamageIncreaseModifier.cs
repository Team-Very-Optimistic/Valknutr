using UnityEngine;

public class DamageIncreaseModifier : SpellModifier
{
    public float damageMultiplier = 2;
    public float speedMultiplier = 0.7f;

    public override void ModifySpell(SpellBase spell)
    {
        base.ModifySpell(spell);
        spell._damage *= damageMultiplier;
        spell._speed *= speedMultiplier;
    }

    public override void UseValue()
    {
        damageMultiplier = damageMultiplier * value;
    }
    
    public override Tooltip GetTooltip()
    {
        return new Tooltip("Damage Amplification" + DefaultModTitle(), $"Increases damage of spell by {damageMultiplier:F} times " +
                                                                       $"while reducing speed by {speedMultiplier:P1}. {DefaultModBody()}");
    }
}