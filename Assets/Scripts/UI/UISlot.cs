using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour, IDropHandler
{
    private UIItem slottedItem;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData != null)
        {
            var uiItem = eventData.pointerDrag.GetComponent<UIItem>();
            if (uiItem == null) return;
            slottedItem = uiItem;
            uiItem.isSlotted = true;
            var rectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            rectTransform.SetParent(transform, true);
            rectTransform.position =
                GetComponent<RectTransform>().position;
        }
    }

    public void Slot(UIItem uiItem)
    {
        slottedItem = uiItem;
        uiItem.isSlotted = true;
        var rectTransform =uiItem.GetComponent<RectTransform>();
        rectTransform.SetParent(transform);
        rectTransform.position =
            GetComponent<RectTransform>().position;
    }
    
    public bool IsSlotted()
    {
        return slottedItem != null && slottedItem.isSlotted;
    }
}