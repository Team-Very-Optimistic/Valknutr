using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{ 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Box collision to follow skinnedMeshRenderer still buggy
        GetComponent<BoxCollider>().transform.position = GetComponent<SkinnedMeshRenderer>().rootBone.transform.position;
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
