using UnityEngine;

public class FireTrap : Trap
{
    protected override void TriggerEvent(Collider other)
    {
        base.TriggerEvent(other);
        var closestPointOnBounds = other.ClosestPointOnBounds(transform.position);
        Fire.SpawnFire(other.gameObject, closestPointOnBounds);
    }
}
