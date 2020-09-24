class SpeedSpellModifier : SpellModifier
{
    public override void ModifySpell(SpellBehavior spell)
    {
        spell._speed += 1f;
        spell._speed *= 2;
    }
}