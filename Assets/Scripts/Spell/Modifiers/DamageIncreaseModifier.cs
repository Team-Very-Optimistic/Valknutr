
using UnityEngine;

[CreateAssetMenu]
public class DamageIncreaseModifier : SpellModifier
{
    public int damageMultiplier = 2;

    // public override void ModifySpell(SpellBase spell)
    // {
    //     base.ModifySpell(spell);
    //     spell.damage *= damageMultiplier;
    // }
    
    public override SpellContext ModifyBehaviour(SpellContext ctx)
    {
        ctx.damage *= damageMultiplier;
        ctx.cooldown *= _cooldownMultiplier;
        return ctx;
    }

    public override void UseValue()
    {
        damageMultiplier = Mathf.RoundToInt(damageMultiplier * value);
    }
    
    public override Tooltip GetTooltip()
    {
        return new Tooltip("Damage Amplification" + DefaultModTitle(), $"Increases damage of spell by {damageMultiplier}. {DefaultModBody()}");
    }
}