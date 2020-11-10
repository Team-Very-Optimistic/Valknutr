using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourBase : Enemy
{
    //Navigation
    protected GameObject player;
    protected NavMeshAgent navMeshAgent;

    //Animation Transitions
    protected Animator animator;
    protected bool isAttacking = false;

    //Knockback parameters
    protected Rigidbody rigidbody;
    protected bool isInKnockback = false;
    public float knockbackVelStoppingThreshold;

    //Bool to allow knockback
    protected bool canKnockback = true;
    public float statsMultiplier = 1;
    
    //For events
    public static Action<EnemyBehaviourBase> OnEnemyStart;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        player = GameManager.Instance._player;
        navMeshAgent.SetDestination(player.transform.position);
        isAttacking = false;
        // OnEnemyStart(this);
    }

    public virtual void ScaleStats(float newMultiplier)
    {
        var multiplier = newMultiplier / statsMultiplier;
        // var modelScale = Mathf.Log(0.1f + newDifficulty/1.2f) * 0.2f + 1f;
        GetComponent<HealthScript>()?.Scale(multiplier);
        GetComponent<Damage>()?.Scale(multiplier);
        // transform.localScale = Vector3.one * modelScale;

        statsMultiplier = newMultiplier;
    }

    public virtual void Update()
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
            if(!isInKnockback)
            {
                if(rigidbody.velocity.magnitude > 0.0f)
                {
                    isInKnockback = true;
                }
            }
            else
            {
                if (rigidbody.velocity.magnitude <= knockbackVelStoppingThreshold)
                {
                    EnableKnockback(false);
                    isInKnockback = false;
                }
            }
        }
    }

    public void EnableKnockback(bool isEnabled)
    {
        if (canKnockback)
        {
            //Temporarily enable/disable navMeshAgent and isKinematic
            navMeshAgent.enabled = !isEnabled;
            rigidbody.isKinematic = !isEnabled;
            rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            isInKnockback = false;
        }
    }
}

