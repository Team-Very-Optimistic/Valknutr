using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell : MonoBehaviour
{
    public GameObject test;
    [SerializeField]
    public SpellBaseType _spellBaseType;
    [SerializeField]
    public List<SpellModifier> _spellModifiers;

    public void Start()
    {
        _spellBaseType = new ProjectileSpell();
        _spellBaseType._objectForSpell = test;
        _spellBaseType._posDiff = transform.forward;
    }
    public void CastSpell()
    {
        _spellBaseType.SpellBehaviour(this);
    }
    
}