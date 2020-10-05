using Doozy.Engine.Extensions;
using UnityEngine;

enum ArcherBehaviourStates
{
    Running,
    DrawBow,
    HoldBow,
    ReleaseBow
}

public class EnemyBehaviour_Archer : EnemyBehaviourBase
{
    //Bow reference
    private GameObject bow;
    private Vector3 bowVerticalOffset = new Vector3(0.0f, 1.4f, 0.0f);

    //Arrow variables
    public GameObject arrowPrefab;
    public float arrowSpeed;

    //Red indicator prefab
    public Material redIndicatorMat;
    private readonly float redIndicatorYOffset = 1.5f;
    private GameObject redIndicatorInstance;

    //Behavior state
    ArcherBehaviourStates archerState;

    //HoldBow State Variables
    public float holdBowDuration;
    private float holdBowTimeElapsed = 0.0f;

    //Wait frames between states - NavMeshAgent SetDestination bug: remaining distance always starts off at zero
    int wait = 0, waitTicks = 1;

    // Start is called before the first frame update
    public override void Start()
    {
        bow = this.gameObject.transform.Find("Erika_Archer_Meshes").Find("Bow").gameObject;
        archerState = ArcherBehaviourStates.Running;
        ResetWaitTicks();
        base.Start(); 
    }

    // Update is called once per frame
    public override void Update()
    {
        //Navigation
        navMeshAgent.SetDestination(player.transform.position);

        if (navMeshAgent.enabled) // Not in knockback
        {
            switch (archerState)
            {
                case ArcherBehaviourStates.Running:
                    {
                        if (--wait > 0) return;

                        //If close enough to player and animator not in transition, switch to draw bow
                        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance && animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
                        {
                            //Trigger anim state
                            ResetAllAnimationTriggers();
                            animator.SetTrigger("ToDraw");

                            //Change enum state
                            archerState = ArcherBehaviourStates.DrawBow;

                            //Stop navMeshAgent
                            navMeshAgent.isStopped = true;

                            ResetWaitTicks();
                        }
                    
                        break;
                    }
                case ArcherBehaviourStates.DrawBow:
                    {
                        if (--wait > 0) return;

                        //Set rotation to player when engaging (use enemy y to prevent vertical rotation)
                        transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));

                        //If player gets out of range, and animator not transitioning, set back to running state
                        if (navMeshAgent.remainingDistance != Mathf.Infinity && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && animator.GetCurrentAnimatorStateInfo(0).IsName("DrawBow"))
                        {
                            //Trigger anim state
                            ResetAllAnimationTriggers();
                            animator.SetTrigger("ToRun");

                            //Change enum state
                            archerState = ArcherBehaviourStates.Running;

                            //Start navMeshAgent
                            navMeshAgent.isStopped = false;

                            ResetWaitTicks();
                        }

                        //Transition to HoldBow done by HoldBow() called by animation event

                        break;
                    }
                case ArcherBehaviourStates.HoldBow:
                    {
                        if (--wait > 0) return;

                        //Set rotation to player when engaging (use enemy y to prevent vertical rotation)
                        transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));

                        //If player gets out of extended range, and animator not transitioning, set back to running state
                        if (navMeshAgent.remainingDistance != Mathf.Infinity && navMeshAgent.remainingDistance > (navMeshAgent.stoppingDistance * 1.25f) && animator.GetCurrentAnimatorStateInfo(0).IsName("HoldBow"))
                        {
                            //Trigger anim state
                            ResetAllAnimationTriggers();
                            animator.SetTrigger("ToRun");

                            //Change enum state
                            archerState = ArcherBehaviourStates.Running;

                            //Start navMeshAgent
                            navMeshAgent.isStopped = false;

                            //Reset holdbow timer
                            holdBowTimeElapsed = 0.0f;

                            //Cancel invoke of red indicator function
                            CancelInvoke("ShowRedIndicator");

                            ResetWaitTicks();
                        }
                        else
                        {
                            holdBowTimeElapsed += Time.deltaTime;

                            //If hold bow timer reached, change to ReleaseBow state
                            if (holdBowTimeElapsed >= holdBowDuration && animator.GetCurrentAnimatorStateInfo(0).IsName("HoldBow"))
                            {
                                //Trigger anim state
                                ResetAllAnimationTriggers();
                                animator.SetTrigger("ToRelease");

                                //Change enum state
                                archerState = ArcherBehaviourStates.ReleaseBow;

                                //Reset holdbow timer
                                holdBowTimeElapsed = 0.0f;

                                ResetWaitTicks();
                            }
                        }

                        break;
                    }
                case ArcherBehaviourStates.ReleaseBow:
                    {
                        if (--wait > 0) return;

                        //Set rotation to player when engaging (use enemy y to prevent vertical rotation)
                        transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));

                        //Transition done to DrawBow/Running by DrawBowOrRun() called by animation event

                        break;
                    }
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

        if(redIndicatorInstance != null)
        {
            redIndicatorInstance.GetComponent<LineRenderer>().SetPosition(0, bow.transform.position + bowVerticalOffset);
            redIndicatorInstance.GetComponent<LineRenderer>().SetPosition(1, player.transform.position);
        }
    }

    public void FireArrow()
    {
        Vector3 fireDirection = (new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z) - this.gameObject.transform.position).normalized;

        GameObject arrow = GameObject.Instantiate(arrowPrefab, bow.transform.position + bowVerticalOffset, Quaternion.LookRotation(fireDirection));
        arrow.GetComponent<EnemyProjectile>().Launch(fireDirection, arrowSpeed);

        HideRedIndicator();
    }

    public void HoldBow()
    {
        //Trigger anim state
        ResetAllAnimationTriggers();
        animator.SetTrigger("ToHold");

        //Change enum state
        archerState = ArcherBehaviourStates.HoldBow;

        //ShowRedIndicator after duration
        Invoke("ShowRedIndicator", holdBowDuration / 1.5f);
    }

    //Called by animation event
    public void DrawBowOrRun()
    {
        ResetAllAnimationTriggers();

        if (navMeshAgent.remainingDistance != Mathf.Infinity && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            //Trigger anim state
            animator.SetTrigger("ToDraw");

            //Change enum state
            archerState = ArcherBehaviourStates.DrawBow;
        }
        else
        {
            //Trigger anim state
            animator.SetTrigger("ToRun");

            //Change enum state
            archerState = ArcherBehaviourStates.Running;

            //Start navMeshAgent
            navMeshAgent.isStopped = false;
        }
    }

    private void ShowRedIndicator()
    {
        redIndicatorInstance = new GameObject("RedIndicator");
        LineRenderer lRend = redIndicatorInstance.AddComponent<LineRenderer>();

        lRend.startColor = Color.red;
        lRend.endColor = Color.red;
        lRend.material = redIndicatorMat;
        lRend.startWidth = 0.02f;
        lRend.endWidth = 0.02f;
        lRend.SetPosition(0, bow.transform.position + bowVerticalOffset);
        lRend.SetPosition(1, player.transform.position);
    }

    private void HideRedIndicator()
    {
        Destroy(redIndicatorInstance);
    }

    private void ResetAllAnimationTriggers()
    {
        animator.ResetTrigger("ToRun");
        animator.ResetTrigger("ToDraw");
        animator.ResetTrigger("ToHold");
        animator.ResetTrigger("ToRelease");
    }

    private void ResetWaitTicks()
    {
        wait = waitTicks;
    }
}
