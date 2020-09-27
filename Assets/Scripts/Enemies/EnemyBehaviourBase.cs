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

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameManager.Instance._player;
    }

    public virtual void Update()
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
            transform.LookAt(player.transform.position);
        }
    }
}