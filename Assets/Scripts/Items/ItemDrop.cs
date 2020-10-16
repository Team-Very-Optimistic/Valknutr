using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public SpellItem _spellItem;
    public Action<ItemDrop> OnPickup;
    private bool isMouseOver = false;
    
    public void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance._player == other.gameObject) //harder comparison precludes clones and shields
        {
            PickUp(other);
            OnPickup?.Invoke(this);
        }
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
        AudioManager.PlaySoundAtPosition("itemPickup", transform.position);
        Inventory.Instance.Add(_spellItem);
        Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        print("mouse over");
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    private void OnDrawGizmos()
    {
        if (isMouseOver)
            Gizmos.DrawSphere(transform.position, 1f);
    }
}
