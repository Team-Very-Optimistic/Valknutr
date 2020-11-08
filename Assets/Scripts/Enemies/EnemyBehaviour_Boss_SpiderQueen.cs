using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour_Boss_SpiderQueen : Enemy
{
    // Navigation
    private GameObject player;
    private NavMeshAgent navMeshAgent;

    // Animation Transitions
    private Animator animator;
    private BossBehaviourStates bossState;
    
    //Summoning
    public int summonEnemiesNum;
    public float summonTime;
    private float summonTimeElapsed;
    private float summonInterval;
    public GameObject spiderPrefab;

    public delegate void EventHandler();
    public static event EventHandler OnBossStart = () => { };
    public static event EventHandler OnBossDeath = () => { };

    //NavMeshAgent SetDestination bug: remaining distance always starts off at zero
    int wait = 0, waitTix = 1;
    int waitMore = 0;

    public void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameManager.Instance._player;
        OnBossStart();
        bossState = BossBehaviourStates.Summoning;
        navMeshAgent.SetDestination(player.transform.position);

        wait = waitTix;
        waitMore = waitTix;
    }

    public void Update()
    {
        if (--wait > 0) return;

        switch (bossState)
        {
            case BossBehaviourStates.Walking:
            {
                //Navigation
                navMeshAgent.SetDestination( 2 *(transform.position - player.transform.position) + RandomNavmeshLocation(5f));

                //Wait frames to ensure navMeshAgent destination set properly
                if (--waitMore > 0) return;

                //If boss close enough to player, start winding up
                if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    //Set Animator booleans
                    ResetAllAnimatorBool();
                    animator.SetBool("isWindingUp", true);

                    //Stop navMeshAgent
                    gameObject.GetComponent<NavMeshAgent>().isStopped = true;

                    //Set internal state
                    bossState = BossBehaviourStates.WindingUp;

                    //Reset waitMore
                    waitMore = waitTix;
                }

                break;
            }
            case BossBehaviourStates.Summoning:
                {
                    summonTimeElapsed += Time.deltaTime;
                    
                    //After summoning
                    if(summonTimeElapsed >= summonTime)
                    {
                        summonInterval = summonTime / (float)(summonEnemiesNum);

                        StartCoroutine(SummonEnemy(summonInterval));
                        //If player is close enough, wind up again
                        if (Vector3.Distance(player.transform.position, transform.position) < navMeshAgent.stoppingDistance)
                        {
                            //Set Animator booleans
                            ResetAllAnimatorBool();
                            animator.SetBool("isWalking", true);

                            //Set internal state
                            bossState = BossBehaviourStates.Walking;
                        }
                        else
                        {
                            //Set Animator booleans
                            ResetAllAnimatorBool();
                            animator.SetBool("isSummoning", true);

                            //Set internal state
                            bossState = BossBehaviourStates.Summoning;

                            //Enable NavMeshAgent
                            navMeshAgent.isStopped = false;
                        }
                        
                        //Reset time
                        summonTimeElapsed = 0.0f;
                    }

                    break;
                }
            case BossBehaviourStates.Death:
                {
                    break;
                }
        }
    }

    private void ResetAllAnimatorBool()
    {
        animator.SetBool("isSummoning", false);
        animator.SetBool("isWalking", false);
    }


    public IEnumerator SummonEnemy(float time)
    {
        yield return new WaitForSeconds(time);
        Vector3 randNavMeshLocation = RandomNavmeshLocation(7.0f);
        
        GameObject.Instantiate(spiderPrefab, randNavMeshLocation, Quaternion.identity);
    }

    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void SetDeathState()
    {
        bossState = BossBehaviourStates.Death;
        OnBossDeath();
    }

    private void OnDestroy()
    {

    }
}
