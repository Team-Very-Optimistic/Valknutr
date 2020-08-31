using UnityEngine;

public abstract class SpellModifier
{
    public abstract void ModifySpell(Spell spell);
}

class SplitShotMod : SpellModifier
{
    public override void ModifySpell(Spell spell)
    {
        
        spell._spellProperties.iterations = 3;
    }
}