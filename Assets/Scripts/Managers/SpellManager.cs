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
    public GameObject aoeEffect;

    public Spell[] GetDefaultSpells()
    {
        int i = 0;
        Spell[] defaultSpells = new Spell[_defaultSpells.Length];
        foreach (var spell in _defaultSpells)
        {
            if (!spell) continue;
            var sp = Instantiate(spell);
            sp.spellBase = Instantiate(spell.spellBase);
            var mods = spell._spellModifiers;
            sp._spellModifiers = new List<SpellModifier>();
            foreach (var m in mods)
            {
                sp.AddModifier(m);
            }

            defaultSpells[i++] = sp;
        }

        _defaultSpells = defaultSpells;
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