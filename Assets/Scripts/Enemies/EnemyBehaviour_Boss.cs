using System;
using UnityEngine;
using UnityEngine.AI;

enum BossBehaviourStates 
{
    Walking,
    WindingUp,
    Stomping,
    Summoning,
    Death
}

public class EnemyBehaviour_Boss : MonoBehaviour
{
    // Navigation
    private GameObject player;
    private NavMeshAgent navMeshAgent;

    // Animation Transitions
    private Animator animator;
    private BossBehaviourStates bossState;

    // Winding up
    public float[] windingUpTime = new float[3];
    private float windingUpTimeRemaining;
    private int windingUpTimeIndex = 0;

    //Summoning
    public int summonEnemiesNum;
    public float summonTime;
    private float summonTimeElapsed;
    private float summonInterval;
    public GameObject paladinPrefab;
    public GameObject archerPrefab;

    //Stomp
    public GameObject stompPrefab;
    public float stompRadius;
    public GameObject rightFeet;

    //NavMeshAgent SetDestination bug: remaining distance always starts off at zero
    int wait = 0, waitTix = 1;
    int waitMore = 0;

    public void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        bossState = BossBehaviourStates.Walking;
        navMeshAgent.SetDestination(player.transform.position);

        summonInterval = summonTime / (float)(summonEnemiesNum);
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
                    navMeshAgent.SetDestination(player.transform.position);

                    if (--waitMore > 0) return;

                    //Animator triggers
                    if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                    {
                        //Setup winding up
                        ResetAllAnimatorBool();
                        animator.SetBool("isWindingUp", true);

                        //Set winding up time
                        windingUpTimeRemaining = windingUpTime[windingUpTimeIndex];

                        //Set internal state
                        bossState = BossBehaviourStates.WindingUp;

                        //Reset waitMore
                        waitMore = waitTix;
                    }

                    break;
                }
            case BossBehaviourStates.WindingUp:
                {
                    windingUpTimeRemaining -= Time.deltaTime;

                    if(windingUpTimeRemaining <= 0.0f)
                    {
                        //Setup stomp
                        ResetAllAnimatorBool();
                        animator.SetBool("isStomping", true);

                        //Set next windingup index pointer
                        windingUpTimeIndex++;
                        if (windingUpTimeIndex == windingUpTime.Length) windingUpTimeIndex = 0;

                        //Set internal state
                        bossState = BossBehaviourStates.Stomping;
                    }

                    break;
                }
            case BossBehaviourStates.Stomping:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
                    {
                        bool shouldSummon = UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f;
                        Debug.Log(shouldSummon);

                        if (shouldSummon)
                        {
                            //Setup summon
                            ResetAllAnimatorBool();
                            animator.SetBool("isSummoning", true);

                            //Set internal state
                            bossState = BossBehaviourStates.Summoning;

                            //Invoke repeating summon
                            InvokeRepeating("SummonEnemy", summonInterval, summonInterval);
                        }
                        else
                        {
                            //If player is close enough, wind up again
                            if (Vector3.Distance(player.transform.position, transform.position) < navMeshAgent.stoppingDistance)
                            {
                                //Setup winding up
                                ResetAllAnimatorBool();
                                animator.SetBool("isWindingUp", true);

                                //Set winding up time
                                windingUpTimeRemaining = windingUpTime[windingUpTimeIndex];

                                //Set internal state
                                bossState = BossBehaviourStates.WindingUp;
                            }
                            else
                            {
                                //Setup summon
                                ResetAllAnimatorBool();
                                animator.SetBool("isWalking", true);

                                //Set internal state
                                bossState = BossBehaviourStates.Walking;
                            } 
                        }
                    }

                    break;
                }
            case BossBehaviourStates.Summoning:
                {
                    summonTimeElapsed += Time.deltaTime;

                    if(summonTimeElapsed >= summonTime)
                    {
                        //If player is close enough, wind up again
                        if (Vector3.Distance(player.transform.position, transform.position) < navMeshAgent.stoppingDistance)
                        {
                            //Setup winding up
                            ResetAllAnimatorBool();
                            animator.SetBool("isWindingUp", true);

                            //Set winding up time
                            windingUpTimeRemaining = windingUpTime[windingUpTimeIndex];

                            //Set internal state
                            bossState = BossBehaviourStates.WindingUp;
                        }
                        else
                        {
                            //Setup summon
                            ResetAllAnimatorBool();
                            animator.SetBool("isWalking", true);

                            //Set internal state
                            bossState = BossBehaviourStates.Walking;
                        }

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
        animator.SetBool("isWindingUp", false);
        animator.SetBool("isStomping", false);
        animator.SetBool("isSummoning", false);
        animator.SetBool("isWalking", false);
    }

    public void SetStomp()
    {
        GameObject go = GameObject.Instantiate(stompPrefab, rightFeet.transform.position, Quaternion.identity);
        go.GetComponent<EnemyBossStomp>().SetMaxScale(stompRadius);
    }

    public void SummonEnemy()
    {
        Vector3 randNavMeshLocation = RandomNavmeshLocation(7.0f);

        bool shouldSummonPaladin = UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f;

        if (shouldSummonPaladin)
        {
            GameObject.Instantiate(paladinPrefab, randNavMeshLocation, Quaternion.identity);
        }
        else
        {
            GameObject.Instantiate(archerPrefab, randNavMeshLocation, Quaternion.identity);
        }
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
        CancelInvoke("SummonEnemy");
    }

    private void OnDestroy()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().SetGameWin();
    }
}
