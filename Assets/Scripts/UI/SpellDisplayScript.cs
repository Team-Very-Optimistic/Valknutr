using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class SpellDisplayScript : MonoBehaviour
{
    public Spell spell;

    public Slider slider;
    public Image[] displays;

    private void Start()
    {
        // set sprite
    }

    private void Update()
    {
        if (spell)
            slider.value = spell.GetCooldownRemainingPercentage();
    }

    public void SetSpell(Spell newSpell)
    {
        spell = newSpell;
        var newSprite = spell ? spell._UIsprite : null;
        foreach (var display in displays)
        {
            display.sprite = newSprite;
        }
    }
}