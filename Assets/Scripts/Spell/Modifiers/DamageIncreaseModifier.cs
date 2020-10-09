public class DamageIncreaseModifier : SpellModifier
{
    private int damageMultiplier = 2;

    public override void ModifySpell(SpellBase spell)
    {
        spell._damage *= damageMultiplier;
    }
}