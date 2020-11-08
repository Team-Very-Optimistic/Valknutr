using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public SpellItem _spellItem;
    public Action<ItemDrop> OnPickup;
    protected Collider playerCollider;
    public bool pickedup;
    protected float mouseOverRadius = 0.5f;

    public void Update()
    {
        // var mousePos = Util.GetMousePositionOnWorldPlane(Camera.main);
        // var pos = transform.position;
        // pos.y = 0;
        var mouseDistance =
            Util.MinDistanceRayPoint(transform.position, Camera.main.ScreenPointToRay(Input.mousePosition));
        if (mouseDistance < mouseOverRadius)
            ShowTooltip();
        else
            CancelTooltip();
    }

    public virtual void ShowTooltip()
    {
        UiManager.ShowTooltip(((ITooltip) _spellItem).GetTooltip(), true);
        UiManager.currentItemDrop = this;
    }

    public void CancelTooltip()
    {
        if (UiManager.currentItemDrop == this)
        {
            UiManager.HideInWorldTooltip();
            UiManager.currentItemDrop = null;
        }
    }

    // public void OnTriggerEnter(Collider other)
    // {
    //     if (GameManager.Instance._player == other.gameObject) //harder comparison precludes clones and shields
    //     {
    //         PlayerEnterHandler(other);
    //     }
    // }

    // public void OnTriggerExit(Collider other)
    // {
    //     if (playerCollider == other)
    //     {
    //         UiManager.HideInWorldTooltip();
    //         playerCollider = null;
    //     }
    //     UiManager.currentItemDrop = null;
    // }

    public virtual void PlayerEnterHandler(Collider other)
    {
        playerCollider = other;
        UiManager.ShowTooltip(((ITooltip) _spellItem).GetTooltip());
        UiManager.currentItemDrop = this;
    }

    public void PickupHandler(ItemDrop itemDrop)
    {
        if (this != itemDrop)
        {
            Destroy(gameObject);
        }
    }

    public virtual void PickUp(GameObject other)
    {
        if (pickedup)
        {
            Debug.Log("picked up already");
            return;
        }

        pickedup = true;
        UiManager.HideInWorldTooltip();
        OnPickup?.Invoke(this);
        AudioManager.PlaySoundAtPosition("itemPickup", transform.position);
        Inventory.Instance.Add(_spellItem);
        Destroy(gameObject);
    }
}