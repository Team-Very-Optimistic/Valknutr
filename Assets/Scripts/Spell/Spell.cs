using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell : SpellItem
{
    #region fields
    
    [SerializeField] protected SpellBehavior spellBehavior;
    [SerializeField] public List<SpellModifier> _spellModifiers = new List<SpellModifier>();
    [HideInInspector] public CastAnimation castAnimation;

    private bool isCooldown;
    public float _coolDown;
    
    #endregion
    
    public void CastSpell(SpellCastData data)
    {
        if (isCooldown) return;

        float totalCooldown = spellBehavior._cooldown;
        spellBehavior.Init();
        spellBehavior._posDiff = data.castDirection;
        spellBehavior.behaviour = () => spellBehavior.SpellBehaviour(this);

        if (_spellModifiers != null && _spellModifiers.Count != 0)
        {
            foreach (var modifier in _spellModifiers)
            {
                modifier.ModifySpell(spellBehavior);
                spellBehavior = modifier.ModifyBehaviour(spellBehavior);
                totalCooldown *= modifier._cooldownMultiplier;
            }
        }

        spellBehavior.Cast();
        _coolDown = totalCooldown;
        GameManager.Instance.StartCoroutine(WaitCooldown(totalCooldown));
    }

    public void AddBaseType(SpellBehavior behaviorType)
    {
        spellBehavior = behaviorType;
        castAnimation = behaviorType.animationType;
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