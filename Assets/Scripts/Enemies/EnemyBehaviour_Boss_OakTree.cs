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
    private float throwAttackStopDist = 10.0f;

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
        switch (bossState)
        {
            case BossOakTreeBehaviourStates.Walking:
                {
                    //Navigation
                    navMeshAgent.SetDestination(player.transform.position);

                    if (--wait > 0) return;

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

                        navMeshAgent.enabled = false;

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

                    //Has ended transition
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("PickUpRock") || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
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

                        navMeshAgent.enabled = true;
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

                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("WalkWithRock"))
                    {
                        navMeshAgent.enabled = true;
                        bossState = BossOakTreeBehaviourStates.Walking;
                        preAnimTriggerSet = false;
                    }

                    break;
                }
        }
    }

    public void SetStomp()
    {
       GameObject.Instantiate(stompPrefab, stompFeet.transform.position, Quaternion.identity);
    }

    private void SetRandomNextAttack(int numAttacks)
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= throwAttackStopDist / 1.5f)
        {
            Debug.Log("Weighted throw");
            SetRandomNextAttackWeightedForThrow();
            return;
        }

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

    private void SetRandomNextAttackWeightedForThrow()
    {
        ResetAllAnimatorTriggers();

        int randInteger = UnityEngine.Random.Range(1, 101);

        if (randInteger <= 25)
        {
            nextAttackState = BossOakTreeBehaviourStates.Attack_Regular;
            navMeshAgent.stoppingDistance = closeAttacksStopDist;

            animator.SetTrigger("ToWalk");
        }
        else if (randInteger <= 50)
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
