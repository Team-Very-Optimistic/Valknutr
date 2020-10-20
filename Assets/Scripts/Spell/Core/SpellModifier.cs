using UnityEngine;
using Object = System.Object;

/// <summary>
/// 
/// </summary>
public abstract class SpellModifier : SpellElement
{
    public float _cooldownMultiplier = 1;
    public float quality;
    [SerializeField]
    private bool init = false;
    public virtual void ModifySpell(SpellBase spell)
    {
        spell._cooldown *= _cooldownMultiplier;
    }

    public virtual SpellBase ModifyBehaviour(SpellBase action)
    {
        //ModifySpell(action);
        return action; // No change
    }

    public abstract void UseQuality();

    public SpellBase Modify(SpellBase spell)
    {
        if (!init)
        {
            UseQuality();
            init = true;
        }
        ModifySpell(spell);
        return ModifyBehaviour(spell);
    }
}