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
    [SerializeField]
    public GameObject _objectForSpell; //spell cast in reference to this object
    
    [HideInInspector]
    public Collider[] _objectsCollided; //colliders that have interacted with object for spell
    
    [HideInInspector]
    public Vector3 _direction; //The vector direction
    
    public Vector3 _offset; //The vector offset for any behaviour
    
    public float _damage = 1;
    
    public float _speed;
    
    public float _scale = 1;
    
    public float _cooldown;
    
    public CastAnimation animationType; //will be mostly ignored by modifiers
    
    [HideInInspector]
    public int _iterations = 1; //Not used yet
    
    [HideInInspector]
    public Action _behaviour; //The behaviour is the one being invoked when spell is cast.
    public QualityManager.Quality _quality;
    [SerializeField]
    protected SpellProperty properties;
    #endregion

    #region PropertyManagement
    [SerializeField]
    protected bool _copied; //why is persistent data so arhghghgh

    [Serializable]
    protected class SpellProperty : ScriptableObject{
        public GameObject _objectForSpell; //spell cast in reference to this object
    
        public Vector3 _offset; //The vector offset for any behaviour
    
        public float _damage;
    
        public float _speed;
    
        public float _scale;
    
        public float _cooldown;
    
        public CastAnimation animationType; //will be mostly ignored by modifiers

        public Action OnDestroyed;
        /*
         * Required or else the data would be modified
         */
        public void OnDestroy()
        {
            OnDestroyed();
        }

        public Action behaviour;
    }
    
    private void ResetValues()
    {
        _objectForSpell = properties._objectForSpell;
        _offset = properties._offset;
        
        _damage = properties._damage;
        
        _speed = properties._speed;
        
        _scale = properties._scale;
        
        _cooldown = properties._cooldown;

        _behaviour = properties.behaviour;
        
        animationType = properties.animationType; //will be mostly ignored by modifiers
    }

    private void CopyValues()
    {
        properties = (SpellProperty) CreateInstance(typeof(SpellProperty));
        properties._objectForSpell = _objectForSpell;
        properties._offset = _offset;
        properties._damage = _damage;
        properties._speed = _speed;
        properties._scale = _scale;
        properties._cooldown = _cooldown;
        properties.animationType = animationType;
        properties.OnDestroyed += ResetValues;
        properties.OnDestroyed += () => _copied = false; //neccessaryarayrayryaryryayasyyasrry
    }
    

    #endregion
    
    public void Cast()
    {
        _behaviour.Invoke();
    }
    
    public void InitializeValues()
    {
        SetValues();
        if (!_copied)
        {
            _copied = true;
            CopyValues();
        }
        else
        {
            ResetValues();
        }
    }
    protected abstract void SetValues();
    //public virtual void AfterModified(){}

    #region Tooltips
    protected virtual string DefaultBaseTitle()
    {
        return $"<Base> (<b><color={QualityManager.GetQualityColor(_quality)}>{_quality}</color></b>)";
    }

   
    protected virtual string DefaultBaseBody()
    {
        return $"\nCooldown: {_cooldown}\nDamage: {_damage} \nSpeed: {_speed}";
    }
    
    public override Tooltip GetTooltip()
    {
        return new Tooltip(DefaultBaseTitle(), DefaultBaseBody());
    }
    

    #endregion
   

    public abstract void SpellBehaviour(Spell spell);

    protected virtual void SpellEffects(bool screenshake, float duration = 0.1f, float intensity = 0.2f, Vector3 pos = default)
    {
        if (screenshake)
        {
            ScreenShakeManager.Instance.ScreenShake(duration, intensity);
        }

        // if (pos != default)
        // {
        //     AudioManager.PlaySoundAtPosition(name, pos);
        //
        // }
        EffectManager.Instance.UseStaffEffect();
    }

}
