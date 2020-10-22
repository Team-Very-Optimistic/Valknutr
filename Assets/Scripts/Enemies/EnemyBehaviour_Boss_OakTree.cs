using System;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.AI;

enum BossOakTreeBehaviourStates 
{
    Walking,
    Attack_Regular,
    Attack_Forward,
    Attack_ThrowRock,
    PickUpRock,
}

public class EnemyBehaviour_Boss_OakTree : MonoBehaviour
{
    // Navigation
    private GameObject player;
    private NavMeshAgent navMeshAgent;

    // Animation Transitions
    private Animator animator;
    private BossOakTreeBehaviourStates bossState;

    /* Attacks */

    private BossOakTreeBehaviourStates nextAttackState;

    // Forward attack
    public GameObject stompPrefab;
    public GameObject stompFeet;

    // Attack stopping distances
    private float closeAttacksStopDist = 3.0f;
    private float throwAttackStopDist = 15.0f;

    public GameObject closeAttackCollider;

    private bool preAnimTriggerSet = false;

    public delegate void EventHandler();
    public static event EventHandler OnBossStart = () => { };
    public static event EventHandler OnBossDeath = () => { };

    //NavMeshAgent SetDestination bug: remaining distance always starts off at zero
    int wait = 0, waitTicks = 5;

    public void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameManager.Instance._player;
        OnBossStart();
        bossState = BossOakTreeBehaviourStates.Walking;
        navMeshAgent.SetDestination(player.transform.position);

        //Start off with choosing between 2 close attacks
        SetRandomNextAttack(2);
        ResetWaitTicks();
    }

    public void Update()
    {
        if (--wait > 0) return;

        switch (bossState)
        {
            case BossOakTreeBehaviourStates.Walking:
                {
                    //Navigation
                    navMeshAgent.SetDestination(player.transform.position);

                    //If boss close enough to player, set to next attack state
                    if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                    {
                        bossState = nextAttackState;
                        ResetWaitTicks();

                        switch(nextAttackState)
                        {
                            case BossOakTreeBehaviourStates.Attack_Regular:
                                {
                                    ResetAllAnimatorTriggers();
                                    animator.SetTrigger("ToRegularAttack");
                                    break;
                                }
                            case BossOakTreeBehaviourStates.Attack_Forward:
                                {
                                    ResetAllAnimatorTriggers();
                                    animator.SetTrigger("ToForwardAttack");
                                    break;
                                }
                            case BossOakTreeBehaviourStates.Attack_ThrowRock:
                                {
                                    ResetAllAnimatorTriggers();
                                    animator.SetTrigger("ToThrowRock");
                                    break;
                                }
                        }

                        preAnimTriggerSet = false;
                    }

                    break;
                }

            case BossOakTreeBehaviourStates.Attack_Regular:
            case BossOakTreeBehaviourStates.Attack_Forward:
            case BossOakTreeBehaviourStates.Attack_ThrowRock:
                {
                    if ((animator.GetCurrentAnimatorStateInfo(0).IsName("ForwardAttack") || animator.GetCurrentAnimatorStateInfo(0).IsName("RegularAttack") ||
                         animator.GetCurrentAnimatorStateInfo(0).IsName("ThrowRock")) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f)
                    {
                        if(!preAnimTriggerSet)
                        {
                            SetRandomNextAttack(3);
                        }
                    }

                    if ((animator.GetCurrentAnimatorStateInfo(0).IsName("ForwardAttack") || animator.GetCurrentAnimatorStateInfo(0).IsName("RegularAttack") ||
                           animator.GetCurrentAnimatorStateInfo(0).IsName("ThrowRock")) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        switch(nextAttackState)
                        {
                            case BossOakTreeBehaviourStates.Attack_Regular:
                            case BossOakTreeBehaviourStates.Attack_Forward:
                                {
                                    bossState = BossOakTreeBehaviourStates.Walking;
                                    break;
                                }

                            case BossOakTreeBehaviourStates.Attack_ThrowRock:
                                {
                                    bossState = BossOakTreeBehaviourStates.PickUpRock;
                                    break;
                                }
                        }

                        preAnimTriggerSet = false;
                    }


                    break;
                }

            case BossOakTreeBehaviourStates.PickUpRock:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("PickUpRock") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f)
                    {
                        if(!preAnimTriggerSet)
                        {
                            ResetAllAnimatorTriggers();
                            animator.SetTrigger("ToWalkWithRock");
                        }
                    }

                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("PickUpRock") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        bossState = BossOakTreeBehaviourStates.Walking;
                        preAnimTriggerSet = false;
                    }

                    break;
                }
        }
    }

    public void SetStomp()
    {
        GameObject go = GameObject.Instantiate(stompPrefab, stompFeet.transform.position, Quaternion.identity);
    }

    private void SetRandomNextAttack(int numAttacks)
    {
        ResetAllAnimatorTriggers();

        int randInteger = UnityEngine.Random.Range(1, numAttacks + 1);
  
        if (randInteger == 1)
        {
            nextAttackState = BossOakTreeBehaviourStates.Attack_Regular;
            navMeshAgent.stoppingDistance = closeAttacksStopDist;

            animator.SetTrigger("ToWalk");
        }
        else if (randInteger == 2)
        {
            nextAttackState = BossOakTreeBehaviourStates.Attack_Forward;
            navMeshAgent.stoppingDistance = closeAttacksStopDist;

            animator.SetTrigger("ToWalk");
        }
        else
        {
            nextAttackState = BossOakTreeBehaviourStates.Attack_ThrowRock;
            navMeshAgent.stoppingDistance = throwAttackStopDist;

            animator.SetTrigger("ToPickUpRock");
        }

        preAnimTriggerSet = true;

        ResetWaitTicks();
    }

    private void ResetWaitTicks()
    {
        wait = waitTicks;
    }

    private void ResetAllAnimatorTriggers()
    {
        animator.ResetTrigger("ToForwardAttack");
        animator.ResetTrigger("ToRegularAttack");
        animator.ResetTrigger("ToPickUpRock");
        animator.ResetTrigger("ToThrowRock");
        animator.ResetTrigger("ToWalkWithRock");
        animator.ResetTrigger("ToWalk");
    }

    private void OnDestroy()
    {
        if(GameManager.Instance)
            GameManager.Instance.SetGameWin();
    }

    public void EnableCloseAttackCollider(int value)
    {
        closeAttackCollider.GetComponent<BoxCollider>().enabled = Convert.ToBoolean(value);
    }

    public void AddRockToBoss()
    {
        Debug.Log("Add rock to boss!");
    }
}
