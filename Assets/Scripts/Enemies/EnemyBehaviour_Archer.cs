using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;


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

    //Arrow variables
    public GameObject arrowPrefab;
    public float arrowSpeed;

    //Red indicator prefab
    public GameObject redIndicatorPrefab;
    private float redIndicatorYOffset = 1.5f;

    //Behavior state
    ArcherBehaviourStates archerState;

    //HoldBow State Variables
    public float holdBowDuration;
    private float holdBowTimeElapsed = 0.0f;

    //Prevent transition states interrupting one another
    int wait = 0, waitTix = 1;

    // Start is called before the first frame update
    public override void Start()
    {
        bow = this.gameObject.transform.Find("Erika_Archer_Meshes").Find("Bow").gameObject;
        archerState = ArcherBehaviourStates.Running;
        base.Start(); 
    }

    // Update is called once per frame
    public override void Update()
    {
        //Navigation
        navMeshAgent.SetDestination(player.transform.position);

        if (navMeshAgent.enabled)
        {
            switch (archerState)
            {
                case ArcherBehaviourStates.Running:
                    {
                        if (--wait > 0) return;

                        if (navMeshAgent.enabled)
                        {
                            //Animation
                            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                            {
                                animator.SetTrigger("ToDraw");
                                archerState = ArcherBehaviourStates.DrawBow;

                                //Stop navMeshAgent
                                navMeshAgent.isStopped = true;
                                wait = 0;
                            }
                        }
                        break;
                    }
                case ArcherBehaviourStates.DrawBow:
                    {
                        if (--wait > 0) return;

                        //Set rotation to player when fighting (use enemy y to prevent rotation)
                        transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));

                        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
                        {
                            animator.SetTrigger("ToRun");
                            archerState = ArcherBehaviourStates.Running;
                            navMeshAgent.isStopped = false;
                            wait = 0;
                        }

                        //Transition to HoldBow done by HoldBow() called by animation event

                        break;
                    }
                case ArcherBehaviourStates.HoldBow:
                    {
                        if (--wait > 0) return;

                        //Set rotation to player when fighting (use enemy y to prevent rotation)
                        transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));

                        if (navMeshAgent.remainingDistance > (navMeshAgent.stoppingDistance * 1.25f))
                        {
                            animator.SetTrigger("ToRun");
                            archerState = ArcherBehaviourStates.Running;
                            navMeshAgent.isStopped = false;
                            holdBowTimeElapsed = 0.0f;
                            wait = 0;
                            CancelInvoke("ShowRedIndicator");
                        }
                        else
                        {
                            holdBowTimeElapsed += Time.deltaTime;

                            if (holdBowTimeElapsed >= holdBowDuration)
                            {
                                animator.SetTrigger("ToRelease");
                                archerState = ArcherBehaviourStates.ReleaseBow;
                                holdBowTimeElapsed = 0.0f;
                                wait = 0;
                            }
                        }

                        break;
                    }
                case ArcherBehaviourStates.ReleaseBow:
                    {
                        if (--wait > 0) return;

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

    }

    public void FireArrow()
    {
        Vector3 fireDirection = (new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z) - this.gameObject.transform.position).normalized;
        Vector3 verticalOffset = new Vector3(0.0f, 1.4f, 0.0f); //bow.transform.position.y returns 0 - Model issue?
        GameObject arrow = GameObject.Instantiate(arrowPrefab, bow.transform.position + verticalOffset, Quaternion.LookRotation(fireDirection));
        arrow.GetComponent<EnemyProjectile>().Launch(fireDirection, arrowSpeed);
    }

    public void HoldBow()
    {
        animator.SetTrigger("ToHold");
        archerState = ArcherBehaviourStates.HoldBow;
        wait = 0;
        Invoke("ShowRedIndicator", holdBowDuration / 1.5f);
    }

    public void DrawBowOrRun()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            animator.SetTrigger("ToDraw");
            archerState = ArcherBehaviourStates.DrawBow;
        }
        else
        {
            animator.SetTrigger("ToRun");
            navMeshAgent.isStopped = false;
            archerState = ArcherBehaviourStates.Running;
        }

        wait = 0;
    }

    public void ShowRedIndicator()
    {
        Debug.Log("showing");
        Vector3 redIndicatorPos = transform.position + new Vector3(0.0f, this.GetComponent<Collider>().bounds.size.y / 2.0f + redIndicatorYOffset, 0.0f);
        GameObject redIndicator = GameObject.Instantiate(redIndicatorPrefab, redIndicatorPos, Quaternion.identity);
        Destroy(redIndicator, holdBowDuration - (holdBowDuration / 1.5f) + 0.5f);
        redIndicator.transform.parent = gameObject.transform;
    }

}
