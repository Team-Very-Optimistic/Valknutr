using UnityEngine;
using Object = System.Object;

public abstract class SpellModifier : SpellElement
{
    public float _cooldownMultiplier = 1;
    
    public virtual void ModifySpell(SpellBehavior spell)
    {
        
    }

    public virtual SpellBehavior ModifyBehaviour(SpellBehavior action)
    {
        return action; // No change
    }
}