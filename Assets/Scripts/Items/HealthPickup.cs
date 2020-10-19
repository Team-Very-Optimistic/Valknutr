using System;
using UnityEngine;
 
///summary
///summary
public class HealthPickup : ItemDrop
{
 
    #region Public Fields

    #endregion
 
    #region Unity Methods

    public override void PickUp(Collider other)
    {
        GameManager.Instance.IncreasePlayerHealth();
        DamageTextManager.SpawnDamage(GameManager.Instance.healthPickupValue, transform.position, Color.green);
        AudioManager.PlaySoundAtPosition("healthPickup", transform.position);
        Destroy(gameObject);
    }

    public override void PlayerEnterHandler(Collider other)
    {
        PickUp(other);
    }

    #endregion
 
    #region Private Methods
    #endregion
}