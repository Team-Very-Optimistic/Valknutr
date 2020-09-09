using System;
using UnityEngine;

[System.Serializable]
public abstract class SpellBaseType
{
    [SerializeField]
    public GameObject _objectForSpell; //can be anything
    [SerializeField]
    public Vector3 _posDiff;
    public Action behaviour;
    public float _cooldown;
    public int _iterations = 1;
    public float _speed;

    public void Cast()
    {
        behaviour.Invoke();
    }
    
    public virtual void Init(){}

    public abstract void SpellBehaviour(Spell spell);
    

}