using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    public Spell spell;
    public Spell movementSpell;

    private void Start()
    {
        var movementSpell1 = new MovementSpell();
        movementSpell1.Init();
        movementSpell._spellBaseType = movementSpell1;
    }

    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Debug.Log("casting spell");
            spell.CastSpell();
        }
        if (Input.GetKeyDown("q"))
        {
           
            movementSpell.CastSpell();
        }
    }
}
