public class ConnectDotsSpellModifier : SpellModifier
{
    public float damageMultiplier = 2;
    public float dropInterval = 0.5f;
    public const int numberToCast = 3;
    public float timeLast = 4f;
    
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
        return new Tooltip("Land transmutation" + DefaultModTitle(), $"Every {dropInterval} a transmuted device is dropped from the object of the spell. Once " +
                                                                     $"{numberToCast} devices have been dropped, they will connect and damage every entity inside " +
                                                                     $"for {damageMultiplier:0.##} the spells damage. Last for {timeLast} seconds. {DefaultModBody()}");
    }
}