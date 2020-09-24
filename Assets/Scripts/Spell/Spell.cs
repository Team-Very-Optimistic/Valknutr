using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell : SpellItem
{
    #region fields
    
    [SerializeField] protected SpellBehavior spellBehavior;
    [SerializeField] public List<SpellModifier> _spellModifiers = new List<SpellModifier>();
    [HideInInspector] public CastAnimation castAnimation;

    public float cooldownMax = 1;
    public float cooldownRemaining = 1;
    private Sprite sprite;
    
    #endregion
    
    public void CastSpell(SpellCastData data)
    {
        if (!IsReady()) return;

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
        cooldownMax = totalCooldown;
        cooldownRemaining = totalCooldown;
    }

    public void AddBaseType(SpellBehavior behaviorType)
    {
        behaviorType.Init();
        spellBehavior = behaviorType;
        castAnimation = behaviorType.animationType;
    }

    public void AddModifier(SpellModifier modifier)
    {
        _spellModifiers.Add(modifier);
    }

    public bool IsReady()
    {
        return cooldownRemaining == 0;
    }

    public void Tick(float t)
    {
        cooldownRemaining = Mathf.Clamp(cooldownRemaining - t, 0, cooldownMax);
    }

    /// <summary>
    /// Returns fraction of cooldown left, i.e. 0 = off cd
    /// </summary>
    /// <returns></returns>
    public float GetCooldownRemainingPercentage()
    {
        if (cooldownMax == 0) return 0;
        return cooldownRemaining / cooldownMax;
    }

    public Sprite GetIcon()
    {
        return sprite;
    }
}