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
    #region Properties
    [HideInInspector]
    public GameObject _objectForSpell; //spell cast in reference to this object
    
    [HideInInspector]
    public Collider[] _objectsCollided; //colliders that have interacted with object for spell
    
    [HideInInspector]
    public Vector3 _direction; //The vector direction
    
    [HideInInspector]
    public Vector3 _offset; //The vector offset for any behaviour
    
    [HideInInspector] public float _damage = 1;
    
    [HideInInspector]
    public float _speed;
    
    [HideInInspector]
    public float _cooldown;
    
    [HideInInspector] public CastAnimation animationType; //will be mostly ignored by modifiers
    
    [HideInInspector]
    public int _iterations = 1; //Not used yet
    
    [HideInInspector]
    public Action behaviour; //The behaviour is the one being invoked when spell is cast.
    
    #endregion

    public void Cast()
    {
        behaviour.Invoke();
    }
    
    public virtual void Init(){}

    public abstract void SpellBehaviour(Spell spell);
    
}