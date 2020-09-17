class FastMod : SpellModifier
{
    public override void ModifySpell(SpellBaseType spell)
    {
        spell._speed *= 2;
    }
}