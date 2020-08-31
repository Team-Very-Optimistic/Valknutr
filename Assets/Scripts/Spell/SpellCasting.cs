using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    public Spell spell;
    public Spell movementSpell;
    public Spell shieldSpell;
    public LayerMask planeLayer;
    private Transform mainCharPos;
    private Camera mainCam;

    private void Start()
    {
        movementSpell = new Spell();
        spell = new Spell();
        shieldSpell = new Spell();
        
        var mod = new SplitShotMod();

        
        var movementSpell1 = new MovementSpell();
        mainCharPos = GameManager.Instance._player.transform;
        movementSpell1.Init();
        movementSpell._spellBaseType = movementSpell1;
        
        var s = new ShieldSpell();
        s.Init();
        shieldSpell._spellBaseType = s;
        shieldSpell._spellModifiers.Add(mod);
        
        var projectile = new ProjectileSpell();
        projectile.Init();
        spell._spellBaseType = projectile;
        spell._spellModifiers.Add(mod);
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int maskOfPlane = 1 << planeLayer;
            if (Physics.Raycast(ray, out hit,30000f, maskOfPlane))
            {
                //one of coordiantes being always zero for aligned plane

                var position = hit.point; //this is relative to 0,0,0


                //relative to a gameObject other
                Vector3 direction = position - mainCharPos.position;
                spell.CastSpell(direction.normalized);

            }
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
           
            shieldSpell.CastSpell();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int maskOfPlane = 1 << planeLayer;
            if (Physics.Raycast(ray, out hit, 30000f, maskOfPlane))
            {
                //one of coordiantes being always zero for aligned plane

                var position = hit.point; //this is relative to 0,0,0


                //relative to a gameObject other
                Vector3 direction = position - mainCharPos.position;
                movementSpell.CastSpell(direction.normalized);
            }
        }
    }
}
    
