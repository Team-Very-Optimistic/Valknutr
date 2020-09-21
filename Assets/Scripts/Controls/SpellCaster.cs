using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class SpellCaster : MonoBehaviour
{
    public Spell[] spells;
    public UiManager uiManager;

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
        if (character.IsDisabled()) return;
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            print("projectile");
            Precast(spells[0]);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            print("shield");
            Precast(spells[1]);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            print("dash");
            Precast(spells[2]);

        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            print("bomb");
            Precast(spells[3]);
        }
    }
    
    private void Precast(Spell spell)
    {
        storedDirection = Util.GetMousePositionOnWorldPlane(mainCam) - transform.position;
        castedSpell = spell;
        transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(storedDirection, Vector3.up), Vector3.up);
        character.SetCastingAnimation(spell.castAnimation);
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
        Gizmos.DrawSphere(Util.GetMousePositionOnWorldPlane(mainCam), 0.5f);
    }
}
    
