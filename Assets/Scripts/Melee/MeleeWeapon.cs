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
        this.GetComponent<BoxCollider>().transform.position = GetComponent<SkinnedMeshRenderer>().rootBone.transform.position;
        this.GetComponent<BoxCollider>().transform.rotation = GetComponent<SkinnedMeshRenderer>().rootBone.transform.rotation;
    }

    public void EnableCollider(bool value)
    {
        this.GetComponent<BoxCollider>().enabled = value;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Enemy"))
        {
            this.gameObject.GetComponentInParent<Damage>().DealDamage(other);
        }
    }
}
