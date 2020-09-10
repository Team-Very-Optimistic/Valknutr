﻿using System;
using UnityEngine;

[System.Serializable]
public abstract class SpellBaseType : SpellElement 
{
    #region Properties
    [HideInInspector]
    public GameObject _objectForSpell; //can be anything
    
    [HideInInspector]
    public GameObject _objectCollided; // can use this to make other people big
    
    [HideInInspector]
    public Vector3 _posDiff;
    
    public Action behaviour;
    
    [HideInInspector]
    public float _cooldown;
    
    [HideInInspector]
    public int _iterations = 1;
    
    [HideInInspector]
    public float _speed;
    
    #endregion

    public void Cast()
    {
        behaviour.Invoke();
    }
    
    public virtual void Init(){}

    public abstract void SpellBehaviour(Spell spell);
    

}