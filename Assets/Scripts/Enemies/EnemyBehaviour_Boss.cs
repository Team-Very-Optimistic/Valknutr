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

                    //Wait frames to ensure navMeshAgent destination set properly
                    if (--waitMore > 0) return;

                    //If boss close enough to player, start winding up
                    if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                    {
                        //Set Animator booleans
                        ResetAllAnimatorBool();
                        animator.SetBool("isWindingUp", true);

                        //Set winding up time (increasing)
                        windingUpTimeRemaining = windingUpTime[windingUpTimeIndex];

                        //Stop navMeshAgent
                        gameObject.GetComponent<NavMeshAgent>().isStopped = true;

                        //Set internal state
                        bossState = BossBehaviourStates.WindingUp;

                        //Reset waitMore
                        waitMore = waitTix;
                    }

                    break;
                }
            case BossBehaviourStates.WindingUp:
                {
                    //Tick down winding up time
                    windingUpTimeRemaining -= Time.deltaTime;

                    //Start stomp on 0 time remaining
                    if(windingUpTimeRemaining <= 0.0f)
                    {
                        //Set Animator booleans
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
                    //After animation ends, change state
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
                    {
                        bool shouldSummon = UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f;

                        if (shouldSummon)
                        {
                            //Set Animator booleans
                            ResetAllAnimatorBool();
                            animator.SetBool("isSummoning", true);

                            //Set internal state
                            bossState = BossBehaviourStates.Summoning;

                            //Invoke repeating summon with delay + interval
                            InvokeRepeating("SummonEnemy", summonInterval, summonInterval);
                        }
                        else
                        {
                            //If player is close enough, wind up again
                            if (Vector3.Distance(player.transform.position, transform.position) < navMeshAgent.stoppingDistance)
                            {
                                //Set Animator booleans
                                ResetAllAnimatorBool();
                                animator.SetBool("isWindingUp", true);

                                //Set winding up time
                                windingUpTimeRemaining = windingUpTime[windingUpTimeIndex];

                                //Set internal state
                                bossState = BossBehaviourStates.WindingUp;
                            }
                            else
                            {
                                //Start walking again

                                //Set Animator booleans
                                ResetAllAnimatorBool();
                                animator.SetBool("isWalking", true);

                                //Set internal state
                                bossState = BossBehaviourStates.Walking;

                                //Enable NavMeshAgent
                                navMeshAgent.isStopped = false;
                            } 
                        }
                    }

                    break;
                }
            case BossBehaviourStates.Summoning:
                {
                    summonTimeElapsed += Time.deltaTime;

                    //After summoning
                    if(summonTimeElapsed >= summonTime)
                    {
                        //If player is close enough, wind up again
                        if (Vector3.Distance(player.transform.position, transform.position) < navMeshAgent.stoppingDistance)
                        {
                            //Set Animator booleans
                            ResetAllAnimatorBool();
                            animator.SetBool("isWindingUp", true);

                            //Set winding up time
                            windingUpTimeRemaining = windingUpTime[windingUpTimeIndex];

                            //Set internal state
                            bossState = BossBehaviourStates.WindingUp;
                        }
                        else
                        {
                            //Set Animator booleans
                            ResetAllAnimatorBool();
                            animator.SetBool("isWalking", true);

                            //Set internal state
                            bossState = BossBehaviourStates.Walking;

                            //Enable NavMeshAgent
                            navMeshAgent.isStopped = false;
                        }

                        //Cancel SummonEnemy invoke
                        CancelInvoke("SummonEnemy");

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
        animator.SetBool("isWindingUp", false);
        animator.SetBool("isStomping", false);
        animator.SetBool("isSummoning", false);
        animator.SetBool("isWalking", false);
    }

    public void SetStomp()
    {
        GameObject go = GameObject.Instantiate(stompPrefab, rightFeet.transform.position, Quaternion.identity);
    }

    public void SummonEnemy()
    {
        Vector3 randNavMeshLocation = RandomNavmeshLocation(7.0f);

        //Randomly summon paladin or arche 
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
        OnBossDeath();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
        {
            LevelManager.Instance.StartNextLevel();
        }
    }
}
