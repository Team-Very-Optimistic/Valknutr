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
    public Action behaviour; //The behaviour is the one being invoked when spell is cast.

    protected SpellProperty properties;
    #endregion

    #region SavedProperties
    private bool _copied;
    public struct SpellProperty {
        public GameObject _objectForSpell; //spell cast in reference to this object
    
        public Collider[] _objectsCollided; //colliders that have interacted with object for spell
        public Vector3 _direction; //The vector direction
    
        public Vector3 _offset; //The vector offset for any behaviour
    
        public float _damage;
    
        public float _speed;
    
        public float _scale;
    
        public float _cooldown;
    
        public CastAnimation animationType; //will be mostly ignored by modifiers
    
        public int _iterations; //Not used yet
    
        public Action behaviour; //The behaviour is the one being invoked when spell is cast.
    }
    #endregion

    public void Cast()
    {
        behaviour.Invoke();
    }

    public void InitializeValues()
    {
        SetValues();
        if(!_copied)
            CopyValues();
        ResetValues();
    }

    private void ResetValues()
    {
        _offset = properties._offset;
        
        _damage = properties._damage;
        
        _speed = properties._speed;
        
        _scale = properties._scale;
        
        _cooldown = properties._cooldown;
        
        animationType = properties.animationType; //will be mostly ignored by modifiers
    }

    private void CopyValues()
    {
        properties = new SpellProperty {_offset = _offset, _damage = _damage, _speed = _speed};
        
        properties._scale =_scale;

        properties._cooldown = _cooldown;
        
        properties.animationType = animationType;
    }

    protected abstract void SetValues();
    //public virtual void AfterModified(){}

    public abstract void SpellBehaviour(Spell spell);
    
}
