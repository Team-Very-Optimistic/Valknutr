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
        spells = SpellManager.Instance.GetDefaultSpells();
        Inventory.Instance._spells.AddRange(spells);
        
        character = GetComponent<ThirdPersonCharacter>();
        mainCam = Camera.main;
        uiManager = UiManager.Instance;
    }

    void Update()
    {
        foreach (var spell in spells)
        {
            if(spell != null)
                spell.Tick(Time.deltaTime * cooldownRate);
        }
    }
    
    public void SetSpell(int index, Spell spell)
    {
        if (index >= spells.Length || index < 0)
        {
            //Debug.LogWarning("Spell index out of range");
            return;
        }
        spells[index] = spell;
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
        if (index >= spells.Length || index < 0)
        {
            Debug.LogWarning("Spell index out of range");
            return;
        }
        if (spells[index] == null)
        {
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
        var animSpeed = spell.GetAnimSpeed();
        character.SetCastingAnimation(spell.castAnimation, animSpeed);
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

    public void ClearSpell(Spell spell)
    {
        for(int i = 0; i < 4; i ++)
        {
            if (spell == spells[i])
            {
                spells[i] = null;
            }
        }
    }
}
    
