using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
        Vector2Int offset = new Vector2Int(85,85); // use this to place the modifiers
        int i = 1;
        foreach (var modiSprite in modifiers)
        {
            if (i == 2) i = -1;
            i++;
            var modiTex = modiSprite.texture;
            for (int x = offset.x * i; x < modiTex.width; x++) {
                for (int y = offset.y * (1 - i) ; y < modiTex.height; y++)
                {
                    var color = modiTex.GetPixel(x, y);
                    float intensity = 0.2f + ((((float) x - 60 * i) / 128) * ((float) y + 60 * i) / 128);
                    intensity *= 0.2f + (0.1f*(1 - i) * (x + y) / 256);
                    color.a = 0.3f + 0.7f * intensity;
                    color *= 0.3f + 0.7f * intensity;
                    color += baseType.texture.GetPixel(x, y)/(1.1f + intensity);
                    newTexture.SetPixel(x, y, color);
                }
            }

            // offset.y += 10;
        }
        newTexture.Apply();
        return Sprite.Create(newTexture, baseType.rect, Vector2.zero);
    }
    
    /// <summary>
    /// Use this method to create the tooltip
    /// </summary>
    public void CreateTooltip(string behav, List<string> modifiers)
    {
        Dictionary<string, string[]> structure =
            new Dictionary<string, string[]>{
                {"structure", new string[]{"BaseTitle", "ModTitle", "FIRSTNAME LASTNAME", "killer of MONSTER", "FIRSTNAME LASTNAME" , "ADJECTIVE_", "MONSTER" }}
        };
            
        string tooltip = behav;
        
        foreach (var modiTooltip in modifiers)
        {
            tooltip += " " + modiTooltip;
        }
        _tooltipMessage = tooltip;
    }

    public override Tooltip GetTooltip()
    {
        // List<string> split = new List<string>(spellBase.GetTooltip().Body.Split(' '));spellBase.GetTooltip().Body.Split(' ');
        // _spellModifiers.ForEach((a) =>
        // {
        //     string[] strings = a.GetTooltip().Body.Split();
        //     split.AddRange(strings);
        // });
        //
        //
        // // _spellModifiers.Aggregate("", (s, modifier) => s + " " + modifier.GetTooltip().Body);
        // // string bodyMessage = spellBase.GetTooltip().Body;
        // string bodyMessage =  split.Aggregate("", (s, s2) =>
        // {
        //     Random.state = new Random.State();
        //     if (Random.value < 0.1)
        //     {
        //         var str = s + " " + s2;
        //         if (Random.value < 0.1)
        //             str += " . . .";
        //         return str;
        //     }
        //     else
        //     {
        //         return s;
        //     }
        // });

        var bodyMessage = spellBase.GetTooltip().Body + _spellModifiers.Aggregate("", (s, modifier) => s + "\n" + modifier.GetTooltip().Title);
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