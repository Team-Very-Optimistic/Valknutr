
using UnityEngine;

[CreateAssetMenu]
public class DamageIncreaseModifier : SpellModifier
{
    private int damageMultiplier = 2;

    public override void ModifySpell(SpellBase spell)
    {
        base.ModifySpell(spell);
        spell._damage *= damageMultiplier;
    }

    public override void UseQuality()
    {
        damageMultiplier = Mathf.RoundToInt(damageMultiplier * quality);
    }
}