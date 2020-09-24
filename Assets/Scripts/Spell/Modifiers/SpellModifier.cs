using UnityEngine;
using Object = System.Object;

public abstract class SpellModifier : SpellElement
{
    public float _cooldownMultiplier = 1;
    
    // for ui tooltips and such
    public string name; 
    public string description;

    public virtual void ModifySpell(SpellBehavior spell)
    {
        
    }

    public virtual SpellBehavior ModifyBehaviour(SpellBehavior action)
    {
        return action; // No change
    }
}