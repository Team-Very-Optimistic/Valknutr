using UnityEngine;

public class TeleportEnemy : EnemyBehaviourBase
{
    private bool teleport = false;
    public void Teleport()
    {
        teleport = true;
        var playerPos = player.transform.position;
        Vector3 nextPosition = 1.2f * (playerPos  - transform.position) +  transform.position + Random.insideUnitSphere;
        navMeshAgent.nextPosition = nextPosition;
        transform.position = nextPosition;
        transform.LookAt(playerPos);
        animator.SetBool("Attacking", true);
    }

    public override void Update()
    {
        if (teleport)
        {
            var playerPos = player.transform.position;
            navMeshAgent.SetDestination( -2 * ( playerPos- transform.position) + transform.position);
            teleport = false;
            animator.SetBool("Attacking", false);
        }
    }
}