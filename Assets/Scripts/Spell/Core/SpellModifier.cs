using UnityEngine;
using Object = System.Object;

/// <summary>
/// 
/// </summary>
public abstract class SpellModifier : SpellElement
{
    public float _cooldownMultiplier = 1;
    
    public virtual void ModifySpell(SpellBase spell)
    {
        
    }

    public virtual SpellBase ModifyBehaviour(SpellBase action)
    {
        return action; // No change
    }
}