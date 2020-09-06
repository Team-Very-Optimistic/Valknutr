using UnityEngine;
using UnityEngine.EventSystems;

class UISlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData != null)
        {
            var uiItem = eventData.pointerDrag.GetComponent<UIItem>();
            if (uiItem == null) return;
            
            uiItem.isSlotted = true;
            var rectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            rectTransform.SetParent(transform, true);
            rectTransform.position =
                GetComponent<RectTransform>().position;
        }
    }
}