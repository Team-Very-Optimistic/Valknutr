using System;
using UnityEngine;

/// <summary>
/// Building block of spells, Behavior and Modifiers. They all inherit from this
/// </summary>
[Serializable]
public abstract class SpellElement : ScriptableObject
{
    public abstract Tooltip GetTooltip();
}