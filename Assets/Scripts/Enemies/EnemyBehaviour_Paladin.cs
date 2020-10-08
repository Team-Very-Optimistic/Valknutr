using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour_Paladin : EnemyBehaviourBase
{
    //Melee Weapon
    private GameObject meleeWeapon;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        meleeWeapon = gameObject.transform.Find("MeleeWeapon").gameObject;
        meleeWeapon.GetComponent<BoxCollider>().enabled = false;
    }
    

    public void EnableMeleeWeaponCollider(int value)
    {
        meleeWeapon.GetComponent<BoxCollider>().enabled = Convert.ToBoolean(value);
    }
}
