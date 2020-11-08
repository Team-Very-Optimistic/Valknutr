using UnityEngine;
using Object = System.Object;

/// <summary>
/// 
/// </summary>
public abstract class SpellModifier : SpellElement
{
    public float _cooldownMultiplier = 1;
    [HideInInspector]
    public float value = 1f;
    
    public virtual void ModifySpell(SpellBase spell)
    {
        spell._cooldown *= _cooldownMultiplier;
    }

    public virtual SpellBase ModifyBehaviour(SpellBase action)
    {
        //ModifySpell(action);
        return action; // No change
    }

    public abstract void UseValue();
    protected virtual string DefaultModTitle()
    {
        return $"<Modifier> (<b><color={QualityManager.GetQualityColor(quality)}>{quality}</color></b>)";
    }

   
    protected virtual string DefaultModBody()
    {
        //todo: make it use "increase or decrease"
        return $" Modifies spell cooldown by {_cooldownMultiplier * 100:F0}%.";
    }
    public override Tooltip GetTooltip()
    {
        return new Tooltip(DefaultModTitle(), DefaultModBody(), $"Level {level}");
    }

    public SpellBase Modify(SpellBase spell)
    {
        ModifySpell(spell);
        return ModifyBehaviour(spell);
    }
}