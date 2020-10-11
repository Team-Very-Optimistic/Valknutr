using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Summon : NoTrigger
{
    private float _damage;
    private float _speed;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isAttacking;
    private GameObject enemy;

    public void Set(float duration, float speed, float damage, float scale)
    {
        Destroy(gameObject, duration);
        _speed = speed;
        _damage = damage;
        transform.localScale *= scale;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemy = GameObject.FindWithTag("Enemy");
    }
    
    public void DamageEnemy(){}

    public void SetPlayerBarrier()
    {
    }

    public void Update()
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
        navMeshAgent.SetDestination(enemy.transform.position);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
        {
            if (navMeshAgent.isStopped) navMeshAgent.isStopped = false;
        }
        else
        {
            if (!navMeshAgent.isStopped) navMeshAgent.isStopped = true;

            //Set rotation to player when fighting (use enemy y to prevent rotation)
            transform.LookAt(new Vector3(enemy.transform.position.x, this.transform.position.y, enemy.transform.position.z));
        }
    }
}