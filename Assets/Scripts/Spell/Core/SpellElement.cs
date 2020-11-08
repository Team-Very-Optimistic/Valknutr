using System;
using UnityEngine;

/// <summary>
/// Building block of spells, Behavior and Modifiers. They all inherit from this
/// </summary>
[Serializable]
public abstract class SpellElement : ScriptableObject, ITooltip
{
    public QualityManager.Quality quality;
    public int level = 1;
    public abstract Tooltip GetTooltip();
}