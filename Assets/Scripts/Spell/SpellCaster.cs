using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class SpellCaster : MonoBehaviour
{
    public float cooldownRate = 1f;
    public Spell[] spells;

    private UiManager uiManager;
    private ThirdPersonCharacter character;
    private Camera mainCam;
    private Spell castedSpell;
    private Vector3 storedDirection;
    
    private void Start()
    {
        spells = SpellManager.GetDefaultSpells();
        
        character = GetComponent<ThirdPersonCharacter>();
        mainCam = Camera.main;
        uiManager = UiManager.Instance;
    }

    void Update()
    {
        foreach (var spell in spells)
        {
            spell.Tick(Time.deltaTime * cooldownRate);
        }
    }

    public void CastSpellAtIndex(int index)
    {
        if (index >= spells.Length)
        {
            Debug.LogWarning("Spell index out of range");
            return;
        }
        castedSpell = spells[index];
        storedDirection = (Util.GetMousePositionOnWorldPlane(mainCam) - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(storedDirection, Vector3.up), Vector3.up);
        character.ClearCastingAnimation();
        var data = new SpellCastData(gameObject, transform.position, storedDirection);
        spells[index].CastSpell(data);
    }

    public void PrecastSpellAtIndex(int index)
    {
        if (index >= spells.Length)
        {
            Debug.LogWarning("Spell index out of range");
            return;
        }
        PrecastInternal(spells[index]);
    }
    
    private void PrecastInternal(Spell spell)
    {
        if (!spell.IsReady()) return;
        
        castedSpell = spell;
        storedDirection = (Util.GetMousePositionOnWorldPlane(mainCam) - transform.position).normalized;
        storedDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(storedDirection, Vector3.up), Vector3.up);
        character.SetCastingAnimation(spell.castAnimation, spell.GetAnimSpeed());
    }

    public void CastPoint()
    {
        // print("cast point! " + castedSpell.name + " (" + storedDirection + ")");
        character.ClearCastingAnimation();
        var data = new SpellCastData(gameObject, transform.position, storedDirection);
        castedSpell.CastSpell(data);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(Util.GetMousePositionOnWorldPlane(mainCam), 0.5f);
    }
}
    
