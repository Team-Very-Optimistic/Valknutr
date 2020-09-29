using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour, IDropHandler
{
    private UIItem slottedItem;
    public bool isBaseSlot;
    public bool isSpellSlot;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData != null)
        {
            var uiItem = eventData.pointerDrag.GetComponent<UIItem>();
            if (uiItem == null) return;
            
            Slot(uiItem);
        }
    }

    public bool Slot(UIItem uiItem)
    {
        if (uiItem._spellItem.isBaseSpell && !isBaseSlot || !uiItem._spellItem.isBaseSpell && isBaseSlot)
        {
            return false;
        }

        if (uiItem.isSlotted)
        {
            foreach (var slot in CraftMenuManager.Instance._itemSlots)
            {
                if (slot != this && uiItem == slot.slottedItem)
                {
                    slot.slottedItem = null;
                    break;
                    
                }
            }
        }
        if (IsSlotted())
        {
            slottedItem.isSlotted = false;
            slottedItem.SetLoose();
        }
        slottedItem = uiItem;
        uiItem.isSlotted = true;
        var rectTransform = uiItem.GetComponent<RectTransform>();
        rectTransform.SetParent(transform);
        rectTransform.position =
            GetComponent<RectTransform>().position;
        return true;
    }

    public UIItem GetSlottedUiItem()
    {
        return slottedItem;
    }
    public SpellItem GetSlottedSpellItem()
    {
        return slottedItem._spellItem;
    }
    
    public bool IsSlotted()
    {
        return slottedItem != null && slottedItem.isSlotted;
    }

    public void Clear()
    {
        Destroy(slottedItem.gameObject);
    }
}