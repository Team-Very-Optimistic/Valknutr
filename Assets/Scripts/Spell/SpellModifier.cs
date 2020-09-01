using System;
using UnityEngine;

public abstract class SpellModifier
{
    public class SpellChain
    {
        public Action spell;

        public SpellChain(Action spell)
        {
            this.spell = spell;
        }
    }

    public abstract void ModifySpell(Spell spell);
    public abstract SpellChain ModifyBehaviour(SpellChain action);
}

class SplitShotMod : SpellModifier
{
    public override void ModifySpell(Spell spell)
    {
        spell._spellProperties.iterations = 1;
    }

    public override SpellChain ModifyBehaviour(SpellChain action)
    {
        Action spell = () =>
        {
            for (int i = 0; i < 2; i++)
            {
                action.spell.Invoke();
            }
        };
        return new SpellChain(spell);
    }
    
}     