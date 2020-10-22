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
        spell.cooldown *= _cooldownMultiplier;
    }

    public virtual SpellContext ModifyBehaviour(SpellContext ctx)
    {
        return ctx; // No change
    }

    public abstract void UseValue();
    protected virtual string DefaultModTitle()
    {
        return $"<Modifier> (<b><color={QualityManager.GetQualityColor(quality)}>{quality}</color></b>)";
    }

   
    protected virtual string DefaultModBody()
    {
        return $"Modifies spell cooldown by {_cooldownMultiplier * 100}%.";
    }
    public override Tooltip GetTooltip()
    {
        return new Tooltip(DefaultModTitle(), DefaultModBody());
    }

    public SpellContext Modify(SpellContext ctx)
    {
        ctx.cooldown *= _cooldownMultiplier;
        return ModifyBehaviour(ctx);
    }
}