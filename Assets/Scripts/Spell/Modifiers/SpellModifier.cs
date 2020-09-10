using UnityEngine;
using Object = System.Object;

public abstract class SpellModifier : SpellElement
{
    public float _cooldownMultiplier = 1;

    public virtual void ModifySpell(SpellBaseType spell)
    {
        
    }

    public virtual SpellBaseType ModifyBehaviour(SpellBaseType action)
    {
        return action; // No change
    }
}