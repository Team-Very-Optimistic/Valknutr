using System;
using UnityEngine;
 
///summary
///summary
public class HealthPickup : ItemDrop
{
 
    #region Public Fields

    #endregion
 
    #region Unity Methods

    public override void PickUp(GameObject other)
    {
        UiManager.HideInWorldTooltip();
        OnPickup?.Invoke(this);
        GameManager.Instance.HealthPickup();
        AudioManager.PlaySoundAtPosition("healthPickup", transform.position);
        Destroy(gameObject);
    }

    // public override void PlayerEnterHandler(Collider other)
    // {
    //     PickUp(other);
    // }
    
    public override void PlayerEnterHandler(Collider other)
    {
        playerCollider = other;
        UiManager.ShowTooltip(new Tooltip("Potion <Consumable>", $"Restores and increase max health by {GameManager.Instance.healthPickupValue}."));
        UiManager.currentItemDrop = this;
    }

    #endregion
 
    #region Private Methods
    #endregion
}