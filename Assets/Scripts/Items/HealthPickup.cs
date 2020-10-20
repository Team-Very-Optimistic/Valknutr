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
        if (other.gameObject == GameManager.Instance._player)
        {
            GameManager.Instance.IncreasePlayerHealth();
            DamageTextManager.SpawnDamage(GameManager.Instance.healthPickupValue, transform.position, Color.green);
            AudioManager.PlaySoundAtPosition("healthPickup", transform.position);
            Destroy(gameObject);
        }
    }

    #endregion
 
    #region Private Methods
    #endregion
}