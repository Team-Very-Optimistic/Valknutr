using UnityEngine;

public class FireTrap : Trap
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        other.gameObject.AddComponent<Fire>().SetInitializer();
    }
}
