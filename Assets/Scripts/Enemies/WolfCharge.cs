using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfCharge : MonoBehaviour
{
    EnemyBehaviour_Wolf enemyBehaviourWolfScript;

    public void Start()
    {
        enemyBehaviourWolfScript = GetComponentInParent<EnemyBehaviour_Wolf>();
    }

    public void OnTriggerEnter(Collider other)
    {
        //Only trigger when charging - Cannot toggle boxcollider + isTrigger
        if (enemyBehaviourWolfScript.GetWolfState() == EnemyBehaviour_Wolf.WolfBehaviourStates.Charging)
        {
            //Check only player's capsule collider
            if (other.gameObject.CompareTag("Player") && other.GetType() == typeof(CapsuleCollider) ||
               (!other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("Player")))
            {
                gameObject.GetComponentInParent<Damage>().DealDamage(other);
            }
        }
    }
}
