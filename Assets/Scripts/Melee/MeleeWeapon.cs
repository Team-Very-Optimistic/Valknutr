using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    BoxCollider weaponBoxCollider;
    SkinnedMeshRenderer weaponSkinnedMeshRenderer;
    public GameObject swordJoint;

    // Start is called before the first frame update
    void Start()
    {
        weaponBoxCollider = GetComponent<BoxCollider>();
        weaponSkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        weaponBoxCollider.transform.position = swordJoint.transform.position;
    }

    //Only used against player
    public void OnTriggerEnter(Collider other)
    {
        //Only apply damage one via capsule collider
        if(other.gameObject.CompareTag("Player") && other.GetType() == typeof(CapsuleCollider))
        {
            this.gameObject.GetComponentInParent<Damage>().DealDamage(other);
        }
    }
}
