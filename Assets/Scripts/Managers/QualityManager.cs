using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class QualityManager : ScriptableObject
{
    /*
     * In order of increasing quality
     */
    public enum Quality
    {
        Simple,
        Intricate,
        Arcane,
        Divine,
        Sanctified
    }

    private float Value(Quality quality, bool positiveBetter = true)
    {
        var value = 1f;
        switch (quality)
        {
            case Quality.Simple:
                value = simpleValue;
                break;
            case Quality.Intricate:
                value = intricateValue;
                break;

            case Quality.Arcane:
                value = arcaneValue;
                break;

            case Quality.Divine:
                value = divineValue;
                break;

            case Quality.Sanctified:
                value = sanctifiedValue;
                break;

            default:
                value = 1;
                break;
        }

        if (!positiveBetter)
        {
            value = 1 / value;
        }

        return value;
    }
    
    public Quality GetQuality(int difficulty)
    {
        var simple = simpleChance + difficulty * simpleAddDifficultyChance;
        var intricate = intricateChance + difficulty * intricateAddDifficultyChance;
        var arcane = arcaneChance + difficulty * arcaneAddDifficultyChance;
        var divine = divineChance + difficulty * divineAddDifficultyChance;
        var sanc = sanctifiedChance + difficulty * sanctifiedAddDifficultyChance;
        var totalChance = simple + intricate + arcane + divine + sanc;
        var rand = Random.Range(0, totalChance);

        if (rand < simple)
        {
            return Quality.Simple;
        }

        rand -= simple;
        if (rand < intricate)
        {
            return Quality.Intricate;
        }
        rand -= intricate;

        if (rand < arcane)
        {
            return Quality.Arcane;
        }
        rand -= arcane;

        if (rand < divine)
        {
            return Quality.Divine;
        }
        rand -= divine;

        if (rand < sanc)
        {
            return Quality.Sanctified;
        }

        return Quality.Sanctified;
    }
    
    // [Range(0f, 1.5f)]
    public float simpleValue;
    public float simpleChance;
    public float simpleAddDifficultyChance;

    
    // [Range(1.5f, 5f)]
    public float intricateValue;
    public float intricateChance;
    public float intricateAddDifficultyChance;

    
    // [Range(5f, 15f)]
    public float arcaneValue;
    public float arcaneChance;
    public float arcaneAddDifficultyChance;

   
    // [Range(15f, 20f)]
    public float divineValue;
    public float divineChance;
    public float divineAddDifficultyChance;

    
    // [Range(20f, 100f)]
    public float sanctifiedValue;
    public float sanctifiedChance;
    public float sanctifiedAddDifficultyChance;

    [Range(0f, 0.99f)]
    public float randomSpread = 0.2f;
    
    public void RandomizeProperties(SpellItem element, Quality quality)
    {
        if (element.isBaseSpell)
        {
            SpellBase spellBase = (SpellBase) element._spellElement;
            spellBase._cooldown /= Mathf.Clamp(Spread() * Value(quality), 1, 10);
            spellBase._damage *= Spread() * Value(quality);
            spellBase._scale *= Spread() * Value(quality);
            spellBase._speed *= Spread() * Value(quality);
            spellBase._quality = quality;
        }
        else
        {
            SpellModifier mod = (SpellModifier) element._spellElement;
            mod._cooldownMultiplier *= Spread();
            mod.value = Spread() * Value(quality);
            mod.quality = quality;
            mod.UseValue();
        }
    }
    
    private float Spread()
    {
        return Random.Range(1 - randomSpread, 1 + randomSpread);
    }

    public static string GetQualityColor(Quality quality)
    {
        switch (quality)
        {
            case Quality.Simple:
                return "red";
                break;
            case Quality.Intricate:
                return "#008080";
                break;

            case Quality.Arcane:
                return "yellow";
                break;

            case Quality.Divine:
                return "#8B008B";
                break;

            case Quality.Sanctified:
                return "#120A8F";
                break;

            default:
                return "teal";
                break;
        }
    }
}

