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
        if (navMeshAgent.enabled)
        {
            //Animation
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }
            animator.SetBool("Attacking", isAttacking);

            //Navigation
            navMeshAgent.SetDestination(player.transform.position);

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
            {
                if (navMeshAgent.isStopped) navMeshAgent.isStopped = false;
            }
            else
            {
                if (!navMeshAgent.isStopped) navMeshAgent.isStopped = true;

                //Set rotation to player when fighting (use enemy y to prevent rotation)
                transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));
            }
        }
        else
        {
            //Check in knockback state before stopping knockback state - Velocity update not neccesarily within same frame of enableknockback
            if (!isInKnockback)
            {
                if (GetComponent<Rigidbody>().velocity.magnitude > 0.0f)
                {
                    isInKnockback = true;
                }
            }
            else
            {
                if (GetComponent<Rigidbody>().velocity.magnitude <= knockbackVelStoppingThreshold)
                {
                    EnableKnockback(false);
                    isInKnockback = false;
                }
            }
        }
    }

    public void EnableMeleeWeaponCollider(int value)
    {
        meleeWeapon.GetComponent<BoxCollider>().enabled = Convert.ToBoolean(value);
    }
}
