using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class SpellDisplayScript : MonoBehaviour
{
    public Spell spell;

    public Slider slider;
    public Image[] renderers;

    private void Start()
    {
        // set sprite
    }

    private void Update()
    {
        slider.value = spell.GetCooldownRemainingPercentage();
    }

    public void SetSpell(Spell newSpell)
    {
        spell = newSpell;
    }
}