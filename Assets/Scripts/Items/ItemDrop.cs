using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public SpellItem _spellItem;
    public Action<ItemDrop> OnPickup;
    protected Collider playerCollider;
    
    public void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance._player == other.gameObject) //harder comparison precludes clones and shields
        {
            PlayerEnterHandler(other);
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (playerCollider != null)
            {
                PickUp(playerCollider);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (playerCollider == other)
        {
            UiManager.HideInWorldTooltip();
            playerCollider = null;
        }
    }

    public virtual void PlayerEnterHandler(Collider other)
    {
        playerCollider = other;
        UiManager.ShowTooltip(((ITooltip)_spellItem).GetTooltip());
    }

    public void PickupHandler(ItemDrop itemDrop)
    {
        if (this != itemDrop)
        {
            Destroy(gameObject);
        }
    }

    public virtual void PickUp(Collider other)
    {
        UiManager.HideInWorldTooltip();
        OnPickup?.Invoke(this);
        AudioManager.PlaySoundAtPosition("itemPickup", transform.position);
        Inventory.Instance.Add(_spellItem);
        Destroy(gameObject);
    }
}
