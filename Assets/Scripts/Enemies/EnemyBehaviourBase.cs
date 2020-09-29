using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourBase : MonoBehaviour
{
    //Navigation
    protected GameObject player;
    protected NavMeshAgent navMeshAgent;

    //Animation Transitions
    protected Animator animator;
    protected bool isAttacking = false;

    //Knockback paramters
    protected bool isInKnockback = false;
    public float knockbackVelStoppingThreshold;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent.SetDestination(player.transform.position);
        isAttacking = false;
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

                //Set rotation to player when fighting
                transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));
            }
        }
        else
        {
            //Check in knockback state before stopping knockback state - Velocity update not neccesarily within same frame of enableknockback
            if(!isInKnockback)
            {
                if(GetComponent<Rigidbody>().velocity.magnitude > 0.0f)
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

    public void EnableKnockback(bool isEnabled)
    {
        //Temporarily enable/disable navMeshAgent and isKinematic
        navMeshAgent.enabled = !isEnabled;
        GetComponent<Rigidbody>().isKinematic = !isEnabled;
        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        isInKnockback = false;
    }
}
