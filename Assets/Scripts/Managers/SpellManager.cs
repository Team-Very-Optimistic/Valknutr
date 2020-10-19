using System.Collections.Generic;
using UnityEngine;

public class SpellManager : Singleton<SpellManager>
{
    /*
     * Used as a database to store prefabs
     */
    public GameObject fireObject;
    public Spell[] _defaultSpells;
    public int maxShields = 10;
    private int currShields = 0;
    public GameObject skeleton;

    public Spell[] GetDefaultSpells()
    {
        int i = 0;
        foreach (var spell in _defaultSpells)
        {
            spell._spellElement = Instantiate(spell._spellElement);
            var mods = spell._spellModifiers;
            spell._spellModifiers = new List<SpellModifier>();
            foreach (var m in mods)
            {
                spell.AddModifier(m);
            }
        }
        return _defaultSpells;
    }

    public bool ShieldFull()
    {
        return currShields >= maxShields;
    }

    public void AddShield()
    {
        currShields++;
    }
    public void RemoveShield()
    {
        currShields--;
    }
}