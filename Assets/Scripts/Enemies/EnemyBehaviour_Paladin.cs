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

        meleeWeapon = this.gameObject.transform.Find("MeleeWeapon").gameObject;
        meleeWeapon.GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void EnableMeleeWeaponCollider(int value)
    {
        meleeWeapon.GetComponent<BoxCollider>().enabled = Convert.ToBoolean(value);
    }
}
