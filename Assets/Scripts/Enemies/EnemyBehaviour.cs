using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    //Navigation
    private GameObject player;
    private NavMeshAgent navMeshAgent;

    //Animation Transitions
    private Animator animator;
    private bool isAttacking = false;

    //Melee Weapon
    private GameObject meleeWeapon;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        meleeWeapon = this.gameObject.transform.Find("MeleeWeapon").gameObject;
        meleeWeapon.GetComponent<BoxCollider>().enabled = false;
    }

    void Update()
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
    }

    public void EnableMeleeWeaponCollider(int value)
    {
        meleeWeapon.GetComponent<BoxCollider>().enabled = Convert.ToBoolean(value);
    }
}
