using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class Spell : SpellItem
{
    #region fields
    
    [SerializeField] protected SpellBehavior spellBehavior;
    [SerializeField] public List<SpellModifier> _spellModifiers = new List<SpellModifier>();
    [HideInInspector] public CastAnimation castAnimation;

    public float cooldownMax = 1;
    public float cooldownRemaining = 1;
    private readonly int hashCode = DateTime.Now.GetHashCode();
    #endregion

    /// <summary>
    /// Use this method to create the sprite based on Base type and modifiers.
    /// Requires read/write enabled texture
    /// Requires RGBA 32bit color
    /// </summary>
    public Sprite CreateProceduralSprite(Sprite baseType, List<Sprite> modifiers)
    {
        int SpriteHeight = 128;
        var newTexture = Texture2D.Instantiate(baseType.texture);
        Vector2Int offset = new Vector2Int(80,80); // use this to place the modifiers
        
        foreach (var modiSprite in modifiers)
        {
            var modiTex = modiSprite.texture;
            for (int x = offset.x; x < modiTex.width; x++) {
                for (int y = offset.y; y < modiTex.height; y++) {
                    newTexture.SetPixel(x, y, modiTex.GetPixel(x, y));
                }
            }

            offset.y += 10;
        }
        newTexture.Apply();
        return Sprite.Create(newTexture, baseType.rect, Vector2.zero);
    }
    
    /// <summary>
    /// Use this method to create the tooltip
    /// </summary>
    public void CreateTooltip(string behav, List<string> modifiers)
    {
        string tooltip = behav;
        
        foreach (var modiTooltip in modifiers)
        {
            tooltip += " " + modiTooltip;
        }
        _tooltipMessage = tooltip;
    }
    
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
    
    public override int GetHashCode()
    {
        return hashCode;
    }

    public float GetAnimSpeed()
    {
        spellBehavior.Init();
        castAnimation = spellBehavior.animationType;

        float oriSpeed = spellBehavior._speed;
        if (_spellModifiers != null && _spellModifiers.Count != 0)
        {
            foreach (var modifier in _spellModifiers)
            {
                modifier.ModifySpell(spellBehavior);
            }
        }
        return spellBehavior._speed / oriSpeed;
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
        return cooldownRemaining <= 0;
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
        if (cooldownMax <= 0) return 0;
        return cooldownRemaining / cooldownMax;
    }
    
}