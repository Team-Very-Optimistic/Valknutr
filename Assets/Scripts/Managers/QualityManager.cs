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

    private float Value(Quality quality, bool positiveBetter)
    {
        var value = 0f;
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
                value = -1;
                break;
        }

        if (!positiveBetter)
        {
            value = 1 / value;
        }

        return value;
    }
    
    private Quality GetQuality()
    {
        var totalChance = simpleChance + intricateChance + arcaneChance + divineChance + sanctifiedChance;
        var rand = Random.Range(0, totalChance);

        if (rand < simpleChance)
        {
            return Quality.Simple;
        }

        rand -= simpleChance;
        if (rand < intricateChance)
        {
            return Quality.Intricate;
        }
        rand -= intricateChance;

        if (rand < arcaneChance)
        {
            return Quality.Arcane;
        }
        rand -= arcaneChance;

        if (rand < divineChance)
        {
            return Quality.Divine;
        }
        rand -= divineChance;

        if (rand < sanctifiedChance)
        {
            return Quality.Sanctified;
        }

        return Quality.Simple;
    }
    
    [Range(0f, 1.5f)]
    public float simpleValue;
    public float simpleChance;
    
    [Range(1.5f, 5f)]
    public float intricateValue;
    public float intricateChance;
    
    [Range(5f, 15f)]
    public float arcaneValue;
    public float arcaneChance;
   
    [Range(15f, 20f)]
    public float divineValue;
    public float divineChance;
    
    [Range(20f, 100f)]
    public float sanctifiedValue;
    public float sanctifiedChance;
    [Range(0f, 0.99f)]
    public float randomSpread = 0.2f;
    
    public void RandomizeProperties(SpellItem element, Quality quality)
    {
        if (element.isBaseSpell)
        {
            SpellBase spellBase = (SpellBase) element._spellElement;
            spellBase._cooldown *= Spread() * Value(quality, false);
            spellBase._damage *= Spread() * Value(quality, true);
            spellBase._scale *= Spread();
            spellBase._speed *= Spread() * Value(quality, true);
        }
        else
        {
            SpellModifier mod = (SpellModifier) element._spellElement;
            mod._cooldownMultiplier *= Spread() * Value(quality, true);
        }
    }
    
    private float Spread()
    {
        return Random.Range(1 - randomSpread, 1 + randomSpread);
    }
}