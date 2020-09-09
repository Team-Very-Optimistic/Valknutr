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

<<<<<<< HEAD
    public UiManager UiManager;
=======
    public UiManager uiManager;
>>>>>>> 6b13fb5c2dc94c45887fd649f273d9f4996b2ec3

    private void Start()
    {
        movementSpell = new Spell();
        spell = new Spell();
        shieldSpell = new Spell();
        
        
        var mod = new SplitShotMod();
        var fire = new FireMod();
        var big = new BigMod();


        
        var movementSpell1 = new MovementSpell();
        mainCharPos = GameManager.Instance._player.transform;
        movementSpell1.Init();
        movementSpell._spellModifiers.Add(mod);
        movementSpell._spellBaseType = movementSpell1;
        movementSpell.AddModifier(fire);
        movementSpell.AddModifier(big);
        
        var s = new ShieldSpell();
        s.Init();
        shieldSpell._spellBaseType = s;
        shieldSpell._spellModifiers.Add(mod);
        shieldSpell._spellModifiers.Add(fire);
        shieldSpell.AddModifier(big);


        var projectile = new ProjectileSpell();
        projectile.Init();
        spell._spellBaseType = projectile;
        spell._spellModifiers.Add(mod);       
        spell.AddModifier(fire);


        mainCam = Camera.main;
<<<<<<< HEAD

        UiManager.keyCodeSkill1 = KeyCode.Mouse0;
        UiManager.keyCodeSkill2 = KeyCode.Q;
        UiManager.keyCodeSkill3 = KeyCode.Mouse1;
=======
        uiManager = UiManager.Instance;
>>>>>>> 6b13fb5c2dc94c45887fd649f273d9f4996b2ec3
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && UiManager.skill1.isCooldown == false)
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
<<<<<<< HEAD
                UiManager.SetSkillCooldown(1, spell._coolDown);
                UiManager.skill1.isCooldown = true;
=======
                uiManager.SetSkillCooldown(1, spell._coolDown);
                uiManager.skill1.isCooldown = true;
>>>>>>> 6b13fb5c2dc94c45887fd649f273d9f4996b2ec3
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Q) && UiManager.skill2.isCooldown == false)
        {
           
            shieldSpell.CastSpell();
<<<<<<< HEAD
            UiManager.SetSkillCooldown(2, spell._coolDown);
            UiManager.skill2.isCooldown = true;
=======
            uiManager.SetSkillCooldown(2, spell._coolDown);
            uiManager.skill2.isCooldown = true;
>>>>>>> 6b13fb5c2dc94c45887fd649f273d9f4996b2ec3
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && UiManager.skill3.isCooldown == false)
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
<<<<<<< HEAD
                UiManager.SetSkillCooldown(3, spell._coolDown);
                UiManager.skill3.isCooldown = true;
=======
                uiManager.SetSkillCooldown(3, spell._coolDown);
                uiManager.skill3.isCooldown = true;
>>>>>>> 6b13fb5c2dc94c45887fd649f273d9f4996b2ec3
            }
        }
    }
}
    
