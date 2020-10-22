using UnityEngine;
using Object = System.Object;

/// <summary>
/// 
/// </summary>
public abstract class SpellModifier : SpellElement
{
    public float _cooldownMultiplier = 1;
    public float value;
    [SerializeField]
    private bool init = false;

    public QualityManager.Quality quality;

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
    protected virtual string DefaultModTitle(SpellContext ctx)
    {
        return $"<Modifier> (<b><color={QualityManager.GetQualityColor(quality)}>{quality}</color></b>)";
    }

   
    protected virtual string DefaultModBody(SpellContext ctx)
    {
        return $"Modifies spell cooldown by {_cooldownMultiplier * 100:F0}%.";
    }
    public override Tooltip GetTooltip(SpellContext ctx)
    {
        return new Tooltip(DefaultModTitle(ctx), DefaultModBody(ctx));
    }

    public SpellBase Modify(SpellBase spell)
    {
        if (!init)
        {
            UseValue();
            init = true;
        }
        ModifySpell(spell);
        return ModifyBehaviour(spell);
    }
}