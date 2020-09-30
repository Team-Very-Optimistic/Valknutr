using UnityEngine;

public class TeleportEnemy : EnemyBehaviourBase
{
    private bool teleport = false;
    private Vector3 targetPoint;
    public Quaternion targetRotation;
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
        targetPoint = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position;
        targetRotation = Quaternion.LookRotation (targetPoint, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
        if (teleport)
        {
            var playerPos = player.transform.position;
            navMeshAgent.SetDestination( Random.Range(-1, 1) * 4 * ( playerPos- transform.position) + transform.position);
            teleport = false;
            animator.SetBool("Attacking", false);
        }
    }
}