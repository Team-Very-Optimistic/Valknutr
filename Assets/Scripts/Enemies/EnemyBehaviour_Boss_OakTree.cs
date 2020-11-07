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

public class EnemyBehaviour_Boss_OakTree : Enemy
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
    public float closeAttacksStopDist = 4.0f;
    public float throwAttackStopDist = 30.0f;

    public GameObject closeAttackCollider;

    //Rock throw
    public GameObject mossRockPrefab;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject mossRockRef;

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

                        //Set rotation to player when engaging (use enemy y to prevent vertical rotation)
                        transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));

                        navMeshAgent.enabled = false;

                        preAnimTriggerSet = false;

                        ResetWaitTicks();
                    }

                    break;
                }

            case BossOakTreeBehaviourStates.Attack_Regular:
            case BossOakTreeBehaviourStates.Attack_Forward:
            case BossOakTreeBehaviourStates.Attack_ThrowRock:
                {
                    if (--wait > 0) return;

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
                                    navMeshAgent.enabled = true;
                                    break;
                                }

                            case BossOakTreeBehaviourStates.Attack_ThrowRock:
                                {
                                    bossState = BossOakTreeBehaviourStates.PickUpRock;
                                    break;
                                }
                        }

                        preAnimTriggerSet = false;

                        ResetWaitTicks();
                    }

                    break;
                }

            case BossOakTreeBehaviourStates.PickUpRock:
                {
                    if (--wait > 0) return;

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
                        ResetWaitTicks();
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
        if(numAttacks > 2)
        {
            if (Vector3.Distance(transform.position, player.transform.position) >= throwAttackStopDist / 1.5f)
            {
                Debug.Log("Weighted throw");
                SetRandomNextAttackWeightedForThrow();
                return;
            }
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
        //Get mean position between two hands
        Vector3 rockPosition = (leftHand.transform.position + rightHand.transform.position) / 2.0f;
        GameObject mossRock = GameObject.Instantiate(mossRockPrefab, rockPosition, Quaternion.identity);
        mossRock.GetComponent<MossRock>().SetHandReferences(leftHand, rightHand);
        mossRockRef = mossRock;
    }

    public void DetatchRockFromBoss()
    {
        mossRockRef.GetComponent<MossRock>().SetTargetDirection(player.transform.position);
        mossRockRef.GetComponent<MossRock>().DetatchFromBoss();
    }

    public void SetDeathState()
    {
        GetComponent<NavMeshAgent>().speed = 0.0f;

        //Disable colliders
        Destroy(closeAttackCollider);
        GetComponent<BoxCollider>().enabled = false;

        Destroy(mossRockRef);
    }
}
