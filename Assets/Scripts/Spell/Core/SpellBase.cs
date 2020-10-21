using System;
using UnityEngine;

/// <summary>
/// Each base spell will be required to use each property as the modifiers will assume that each spell base has use each property.
/// While some spells will not make sense with using all these properties, they could have multiple interactions of sorts with the normal base spell
/// using some of the properties, and the other properties could be controlled by modifiers.
/// </summary>
[Serializable]
public abstract class SpellBase : SpellElement
{
    public static Transform _player; //allows all spell element to have easy reference to player
    
    #region Properties
    
    [HideInInspector]
    public Collider[] _objectsCollided; //colliders that have interacted with object for spell

    public QualityManager.Quality _quality;
    public GameObject objectForSpell; //spell cast in reference to this object
    public float damage = 1;
    public float speed = 1;
    public float scale = 1;
    public float cooldown = 1;
    public float _duration = 1;
    public Vector3 direction; //The vector direction
    public Vector3 offset; //The vector offset for any behaviour
    public CastAnimation animationType; //will be mostly ignored by modifiers
    public Action<SpellContext> behaviour; //The behaviour is the one being invoked when spell is cast.
    #endregion

    #region PropertyManagement
    
    #endregion

    public virtual SpellContext GetContext()
    {
        behaviour = SpellBehaviour;
        return new SpellContext(behaviour, this, default, objectForSpell, damage, speed, scale, cooldown, direction, offset, _duration, 1);
    }
    
    public void Cast(SpellContext ctx)
    {
        behaviour.Invoke(ctx);
    }

    #region Tooltip

    protected virtual string DefaultBaseTitle(SpellContext ctx)
    {
        return $"<Base> (<b><color={QualityManager.GetQualityColor(_quality)}>{_quality}</color></b>)";
    }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
    
    protected virtual string DefaultBaseBody(SpellContext ctx)
    {
        return $"\nCooldown: {ctx.cooldown}\nDamage: {ctx.damage} \nSpeed: {ctx.speed}";
    }
    
    public override Tooltip GetTooltip(SpellContext ctx)
    {
        return new Tooltip(DefaultBaseTitle(ctx), DefaultBaseBody(ctx));
    }

    #endregion
    

    public abstract void SpellBehaviour(SpellContext ctx);

    protected virtual void SpellEffects(bool screenshake, float duration = 0.1f, float intensity = 0.2f, Vector3 pos = default)
    {
        if (screenshake)
        {
            ScreenShakeManager.Instance.ScreenShake(duration, intensity);
        }
        EffectManager.Instance.UseStaffEffect();
    }

}