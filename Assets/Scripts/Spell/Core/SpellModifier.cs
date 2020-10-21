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
    protected virtual string DefaultModTitle()
    {
        return $"<Modifier> (<b><color=\"{QualityManager.GetQualityColor(quality)}\"> {quality}</color></b>)";
    }

    public override Tooltip GetTooltip()
    {
        return new Tooltip(DefaultModTitle(), DefaultModBody());
    }

    protected virtual string DefaultModBody()
    {
        return $"Changes spell cooldown by {(1 - this._cooldownMultiplier) * 100}%.";
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