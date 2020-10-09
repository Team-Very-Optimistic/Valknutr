using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public SpellItem _spellItem;

    private bool isMouseOver = false;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp();
        }
    }

    public void PickUp()
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
