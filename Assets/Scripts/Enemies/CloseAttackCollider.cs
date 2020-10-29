using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class CloseAttackCollider : MonoBehaviour
{
    private Damage damageScript;

    // Start is called before the first frame update
    void Start()
    {
        damageScript = this.GetComponentInParent<Damage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && other.GetType() == typeof(CapsuleCollider))
        {
           damageScript.DealDamage(other);
        }
    }
}
