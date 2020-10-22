using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class Spell : SpellItem
{
    #region fields
    
    [SerializeField] public SpellBase spellBase;
    [SerializeField] public List<SpellModifier> _spellModifiers = new List<SpellModifier>();
    [HideInInspector] public CastAnimation castAnimation;
    
    public float cooldownMax = 1;
    [HideInInspector]
    public float cooldownRemaining = 1;
    private readonly int hashCode = DateTime.Now.GetHashCode();
    private string _tooltipMessage;
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

    public override Tooltip GetTooltip()
    {
        string bodyMessage = spellBase.GetTooltip().Body +
                             _spellModifiers.Aggregate("", (s, modifier) => s + " " + modifier.GetTooltip().Body);
        return new Tooltip(spellBase.GetTooltip().Title, bodyMessage);
        
        /*
         * Damaging Multi-Projectile of Speed
         * CD: 0.6
         * Dmg: 2
         * Count: 3
         *
         * Fires a projectile. Affected spells will be faster. Repeats spell 3 times with 50% effect. Spell deals 100% increased damage.
         *
         * Flavor text
         */
    }
    
    public void CastSpell(SpellCastData data)
    {
        if (!IsReady()) return;

        float totalCooldown = spellBase._cooldown;
        spellBase.InitializeValues();
        spellBase._direction = data.castDirection;
        spellBase._behaviour = () => spellBase.SpellBehaviour(this);

        if (_spellModifiers != null && _spellModifiers.Count != 0)
        {
            foreach (var modifier in _spellModifiers)
            {
                spellBase = modifier.Modify(spellBase);
                totalCooldown *= modifier._cooldownMultiplier;
            }
        }

        spellBase.Cast();
        cooldownMax = totalCooldown;
        cooldownRemaining = totalCooldown;
    }
    
    public override int GetHashCode()
    {
        return hashCode;
    }

    public float GetAnimSpeed()
    {
        spellBase.InitializeValues();
        castAnimation = spellBase.animationType;

        float oriSpeed = spellBase._speed;
        if (_spellModifiers != null && _spellModifiers.Count != 0)
        {
            foreach (var modifier in _spellModifiers)
            {
                modifier.ModifySpell(spellBase);
            }
        }
        return spellBase._speed / oriSpeed;
    }

    public void AddBaseType(SpellBase baseType)
    {
        SpellBase copy = Instantiate(baseType);
        copy.InitializeValues();
        spellBase = copy;
        castAnimation = copy.animationType;
    }

    public void AddModifier(SpellModifier modifier)
    {
        SpellModifier copy = Instantiate(modifier);
        _spellModifiers.Add(copy);
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