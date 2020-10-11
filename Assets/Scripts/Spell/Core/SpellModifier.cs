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
        spell._cooldown *= _cooldownMultiplier;
    }

    public virtual SpellBase ModifyBehaviour(SpellBase action)
    {
        //ModifySpell(action);
        return action; // No change
    }

    public SpellBase Modify(SpellBase spell)
    {
        ModifySpell(spell);
        return ModifyBehaviour(spell);
    }
}