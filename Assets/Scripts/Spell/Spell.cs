using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell
{
    [SerializeField]
    public SpellBaseType _spellBaseType;
    [SerializeField]
    public List<SpellModifier> _spellModifiers =  new List<SpellModifier>() ;

    private bool isCooldown;
    
    public void CastSpell(Vector3 mouseDirection = new Vector3())
    {
        if (isCooldown) return; //On cooldown
        
        float totalCooldown = _spellBaseType._cooldown;
        _spellBaseType._posDiff = mouseDirection;
        _spellBaseType.behaviour = () => _spellBaseType.SpellBehaviour(this);
        
        if (_spellModifiers != null && _spellModifiers.Count != 0)
        {
            foreach (var modifier in _spellModifiers)
            {
                modifier.ModifySpell(_spellBaseType);
                _spellBaseType = modifier.ModifyBehaviour(_spellBaseType);
                totalCooldown *= modifier._cooldownMultiplier;
            }

        }
        _spellBaseType.Cast();
        GameManager.Instance.StartCoroutine(WaitCooldown(totalCooldown));
    }

    public void AddBaseType(SpellBaseType baseType)
    {
        _spellBaseType = baseType;
    }
    public void AddModifier(SpellModifier modifier)
    {
        _spellModifiers.Add(modifier);
    }

    IEnumerator WaitCooldown(float cooldown)
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }
}