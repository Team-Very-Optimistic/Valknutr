using System;
using UnityEngine;
 
///summary
///summary
public class HealthPickup : ItemDrop
{
 
    #region Public Fields

    public float healAmount = 1f;
    #endregion
 
    #region Unity Methods

    public override void PickUp(GameObject other)
    {
        UiManager.HideInWorldTooltip();
        OnPickup?.Invoke(this);
        GameManager.Instance.HealthPickup(healAmount);
        AudioManager.PlaySoundAtPosition("healthPickup", transform.position);
        Destroy(gameObject);
    }

    // public override void PlayerEnterHandler(Collider other)
    // {
    //     PickUp(other);
    // }
    public override void ShowTooltip()
    {
        UiManager.ShowTooltip(new Tooltip("Potion <Consumable>", $"Restores and increase max health by {healAmount}."), true);
        UiManager.currentItemDrop = this;
    }

    public override void PlayerEnterHandler(Collider other)
    {
        playerCollider = other;
        UiManager.ShowTooltip(new Tooltip("Potion <Consumable>", $"Restores and increase max health by {healAmount}."));
        UiManager.currentItemDrop = this;
    }

    #endregion
 
    #region Private Methods
    #endregion
}