using UnityEngine;

/// <summary>
/// Vessel class for spells that don't use triggers
/// </summary>
public class NoTrigger : TriggerEventHandler
{
    public override void TriggerEvent(Collider other)
    {
    }
}