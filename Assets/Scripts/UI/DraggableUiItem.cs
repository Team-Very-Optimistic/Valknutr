using UnityEngine;
using UnityEngine.EventSystems;

class DraggableUiItem : UIItem,  IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public void Update()
    {
        if (selected && Input.GetButtonDown("Submit"))
        {
            Debug.Log("Submit");
            var itemSlots = CraftMenuManager.Instance._itemSlots;

            foreach (var slot in itemSlots)
            {
                if (!slot.IsSlotted())
                {
                    var slotted = slot.Slot(this);
                    if (slotted)
                    {
                        break;
                    }
                }
            }
        }
    }
    
    
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;
        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            Debug.Log("Drop item");
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _siblingIndex = transform.GetSiblingIndex();
        transform.SetParent(CraftMenuManager.Instance.transform);
        _canvasGroup.blocksRaycasts = false;
        isSlotted = false;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetLoose();
    }

}