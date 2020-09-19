using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class SpellCastingControl : MonoBehaviour
{
    public Spell projectileSpell;
    public Spell movementSpell;
    public Spell shieldSpell;
    public LayerMask planeLayer;
    private ThirdPersonCharacter character;
    private Camera mainCam;

    public UiManager uiManager;

    private Spell castedSpell;
    private Vector3 storedDirection;
    
    private void Start()
    {
        character = GetComponent<ThirdPersonCharacter>();
        movementSpell = new Spell();
        projectileSpell = new Spell();
        shieldSpell = new Spell();
        
        
        var mod = new SplitShotMod();
        var fire = new FireMod();
        var big = new BigMod();

        var movementSpell1 = new MovementSpell();
        // mainCharPos = GameManager.Instance._player.transform;
        movementSpell1.Init();
        movementSpell._spellModifiers.Add(mod);
        movementSpell._spellBaseType = movementSpell1;
        //.AddModifier(fire);
        
        var s = new ShieldSpell();
        s.Init();
        shieldSpell._spellBaseType = s;
        //shieldSpell.AddModifier(big);
        //shieldSpell._spellModifiers.Add(mod);
        //shieldSpell._spellModifiers.Add(fire);


        var projectile = new ProjectileSpell();
        projectile.Init();
        projectileSpell._spellBaseType = projectile;
        //spell._spellModifiers.Add(mod);       
       // spell.AddModifier(fire);
        //spell.AddModifier(ScriptableObject.CreateInstance<PhaseMod>());
        
        var explosionSpell = ScriptableObject.CreateInstance<ExplosionSpell>();
        explosionSpell.Init();
        var exploSpell = ScriptableObject.CreateInstance<Spell>();
        exploSpell._spellBaseType = explosionSpell;
        // exploSpell._spellModifiers.Add(mod);       
        // exploSpell.AddModifier(fire);
        // exploSpell.AddModifier(ScriptableObject.CreateInstance<PhaseMod>());
        Inventory.Instance._spells.Add(exploSpell);
        mainCam = Camera.main;
        uiManager = UiManager.Instance;
    }

    void Update()
    {
        if (character.IsDisabled()) return;
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Precast(projectileSpell);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Precast(shieldSpell);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            storedDirection = Util.GetMousePositionOnWorldPlane(mainCam);
            castedSpell = movementSpell;
            Precast(movementSpell);

        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Precast(Inventory.Instance._spells[Inventory.Instance._spells.Count - 1]);
        }
    }
    
    private void Precast(Spell spell)
    {
        storedDirection = Util.GetMousePositionOnWorldPlane(mainCam) - transform.position;
        castedSpell = spell;
        transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(storedDirection, Vector3.up), Vector3.up);
        character.SetCastingAnimation(spell.animationType);
    }

    public void CastPoint()
    {
        // print("cast point! " + castedSpell.name + " (" + storedDirection + ")");
        character.ClearCastingAnimation();
        projectileSpell.CastSpell(storedDirection.normalized);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Util.GetMousePositionOnWorldPlane(mainCam), 0.5f);
    }
}
    
